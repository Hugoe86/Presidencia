using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Presidencia.Bandeja_Solicitudes_Tramites.Negocio;
using Presidencia.Sessiones;
using Presidencia.Plantillas_Word;
using Presidencia.Registro_Peticion.Datos;
using System.IO;
using DocumentFormat.OpenXml.Wordprocessing;
using Presidencia.Constantes;
using System.Net.Mail; 
using Presidencia.Catalogo_Tramites.Negocio;
using Presidencia.Operacion_Predial_Dias_Inhabiles.Negocio;
using Presidencia.Ventanilla_Consultar_Tramites.Negocio;
using Presidencia.Orden_Territorial_Formato_Ficha_Inspeccion.Negocio;
using Presidencia.Solicitud_Tramites.Negocios;
using Presidencia.Ordenamiento_Territorial_Zonas.Negocio;
using Presidencia.Catalogo_Ordenamiento_Territorial_Parametros.Negocio;
using DocumentFormat.OpenXml.Packaging;
using openXML_Wp = DocumentFormat.OpenXml.Wordprocessing;
using Presidencia.Operacion_Predial_Pagos_Instit_Externas.Negocio;
using System.Text;
using System.Xml;
using AjaxControlToolkit;
using Presidencia.Dependencias.Negocios;
using Presidencia.Ordenamiento_Territorial_Inspectores.Negocio;
using Presidencia.Catalogo_Tramites_Parametros.Negocio;
using Presidencia.Operacion_Resumen_Predio.Negocio;
using System.Collections.Generic;
using DocumentFormat.OpenXml;
using System.IO.Packaging;
using System.Text.RegularExpressions;
using Presidencia.Ayudante_Plantilla_Word;
using Presidencia.Reportes;
using System.Net.Mime;
using System.Web.Security;
using Presidencia.Orden_Territorial_Bitacora_Documentos.Negocio;
using System.Drawing;

public partial class paginas_Tramites_Ope_Bandeja_Tramites : System.Web.UI.Page
{
    #region Page_Load

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Page_Load
    ///DESCRIPCIÓN: Metodo que se carga cada que ocurre un PostBack de la Página
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 14/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Mensaje_Error(false, "");

            if (!IsPostBack)
            {
                //Variable para mantener la session activa.
                Session["Activa"] = true;
                if (Request.QueryString["Solicitud"] != null)
                {
                    Hdf_Solicitud_ID.Value = Request.QueryString["Solicitud"];
                    Grid_Bandeja_Entrada_SelectedIndexChanged(sender, null);
                }
                else if (Session["Solicitud"] != null)
                {
                    Hdf_Solicitud_ID.Value = Session["Solicitud"].ToString();
                    Grid_Bandeja_Entrada_SelectedIndexChanged(sender, null);
                    Session.Remove("Solicitud");
                }
                else
                {
                    Grid_Bandeja_Entrada.Columns[1].Visible = false;
                    Grid_Datos_Tramite.Columns[0].Visible = false;
                    Grid_Datos_Tramite.Columns[1].Visible = false;
                    Grid_Documentos_Tramite.Columns[0].Visible = false;
                    Grid_Documentos_Tramite.Columns[1].Visible = false;
                    Grid_Documentos_Tramite.Columns[3].Visible = false;
                    Grid_Plantillas.Columns[1].Visible = false;
                    Grid_Plantillas.Columns[3].Visible = false;
                    Grid_Marcadores_Platilla.Columns[0].Visible = false;
                    Grid_Documentos_Seguimiento.Columns[1].Visible = false;

                    if (Session["GRID_BANDEJA_TRAMITES"] != null)
                    {
                        Llenar_Grid_Solicitudes_Tramites(0);
                        Hdf_Habilitar_Evaluar.Value = "false";
                        //Btn_Evaluar.Visible = false;
                    }
                    else
                    {
                        Consultar_Solicitudes_Tramites();
                        Hdf_Habilitar_Evaluar.Value = "true";
                        //Btn_Evaluar.Visible = true;
                    }
                    
                    
                    Habilitar_Boton_Modificar_Zona();
                    Mostrar_Filtro_Fecha();
                    Cargar_Combo_Nombres();
                    ViewState["SortDirection"] = "DESC";
                }
            }
            //String Ventana_Modal = "Abrir_Ventana_Modal('../Atencion_Ciudadana/Ventanas_Emergente/Frm_Busqueda_Avanzada_Dependencias.aspx', 'center:yes;resizable:no;status:no;dialogWidth:680px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
            //Btn_Buscar_Dependencia.Attributes.Add("onclick", Ventana_Modal);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }

    #endregion

    #region Metodos

    #region Grid Solicitudes Tramites

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Consultar_Solicitudes_Tramites
    ///DESCRIPCIÓN: Hace una consulta a la Base de Datos para obtener los tramites
    ///             para el Empleado.
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 14/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************      
    private void Consultar_Solicitudes_Tramites()
    {
        Cls_Ope_Bandeja_Tramites_Negocio Tramites = new Cls_Ope_Bandeja_Tramites_Negocio();
        Cls_Cat_Ort_Parametros_Negocio Obj_Parametros = new Cls_Cat_Ort_Parametros_Negocio();
        String Dependencia_ID_Ordenamiento = "";
        String Dependencia_ID_Ambiental = "";
        String Dependencia_ID_Urbanistico = "";
        String Dependencia_ID_Inmobiliario = "";
        String Dependencia_ID_Catastro = "";
        String Rol_Director_Ordenamiento = "";
        String Director_Ambiental = "";
        String Director_Urbanistico = "";
        String Director_Fraccionamientos = "";
        String Inspector_Ordenamiento = "";
        DataTable Tabla = new DataTable();
        Boolean Estatus_Dependencia = false;

        try
        {
            // consultar parámetros
            Obj_Parametros.Consultar_Parametros();

            //  para las dependencias
            if (!string.IsNullOrEmpty(Obj_Parametros.P_Dependencia_ID_Ordenamiento))
                Dependencia_ID_Ordenamiento = Obj_Parametros.P_Dependencia_ID_Ordenamiento;

            if (!string.IsNullOrEmpty(Obj_Parametros.P_Dependencia_ID_Ambiental))
                Dependencia_ID_Ambiental = Obj_Parametros.P_Dependencia_ID_Ambiental;

            if (!string.IsNullOrEmpty(Obj_Parametros.P_Dependencia_ID_Urbanistico))
                Dependencia_ID_Urbanistico = Obj_Parametros.P_Dependencia_ID_Urbanistico;

            if (!string.IsNullOrEmpty(Obj_Parametros.P_Dependencia_ID_Inmobiliario))
                Dependencia_ID_Inmobiliario = Obj_Parametros.P_Dependencia_ID_Inmobiliario;

            if (!string.IsNullOrEmpty(Obj_Parametros.P_Dependencia_ID_Catastro))
                Dependencia_ID_Catastro = Obj_Parametros.P_Dependencia_ID_Catastro;

            // roles
            if (!string.IsNullOrEmpty(Obj_Parametros.P_Rol_Director_Ordenamiento))
                Rol_Director_Ordenamiento = Obj_Parametros.P_Rol_Director_Ordenamiento;

            if (!string.IsNullOrEmpty(Obj_Parametros.P_Rol_Director_Ambiental))
                Director_Ambiental = Obj_Parametros.P_Rol_Director_Ambiental;

            if (!string.IsNullOrEmpty(Obj_Parametros.P_Rol_Director_Fraccionamientos))
                Director_Fraccionamientos = Obj_Parametros.P_Rol_Director_Fraccionamientos;

            if (!string.IsNullOrEmpty(Obj_Parametros.P_Rol_Director_Urbana))
                Director_Urbanistico = Obj_Parametros.P_Rol_Director_Urbana;

            
            if (!string.IsNullOrEmpty(Obj_Parametros.P_Rol_Inspector_Ordenamiento))
                Inspector_Ordenamiento = Obj_Parametros.P_Rol_Inspector_Ordenamiento;

            Limpiar_Formulario(); // Se limpia el Formulario
            Configuracion_Catalogo(false); //Se inhabilitan los componentes
            Habilitar_Comentarios(false);
            Session.Remove("GRID_BANDEJA_TRAMITES"); // Se elimina la variable de Session que contiene los datos del Grid

            if (Chk_Fechas.Checked == true)
            {
                if (Verificar_Fecha())
                {
                    Tramites.P_Dependencia_ID = Cls_Sessiones.Dependencia_ID_Empleado;
                    Tramites.P_Fecha_Inicio = Txt_Fecha_Inicio.Text;
                    Tramites.P_Estatus = Cmb_Buscar_Solicitudes_Estatus.SelectedValue;
                    Tramites.P_Fecha_Fin = Txt_Fecha_Fin.Text;
                    Tabla = Tramites.Consultar_Solicitud_Director_Ordenamiento();
                }
            }

            else
            {
                if (Cls_Sessiones.Empleado_ID != "")
                {
                    //  filtro para el rol del director de ordenamiento
                    if (Rol_Director_Ordenamiento == Cls_Sessiones.Rol_ID)
                    {
                        Tramites.P_Empleado_ID = Cls_Sessiones.Empleado_ID;
                        Tramites.P_Rol_ID = Cls_Sessiones.Rol_ID;
                    }
                    else
                    {
                        Tramites.P_Empleado_ID = Cls_Sessiones.Empleado_ID;
                        Tramites.P_Dependencia_ID = String.Empty;
                    }
                }

                if (Cls_Sessiones.Dependencia_ID_Empleado == Dependencia_ID_Ordenamiento
                    || Cls_Sessiones.Dependencia_ID_Empleado == Dependencia_ID_Ambiental
                    || Cls_Sessiones.Dependencia_ID_Empleado == Dependencia_ID_Inmobiliario
                    || Cls_Sessiones.Dependencia_ID_Empleado == Dependencia_ID_Urbanistico)
                {
                    Tramites.P_Dependencia_ID = Cls_Sessiones.Dependencia_ID_Empleado;
                    Estatus_Dependencia = true;

                    if (Cls_Sessiones.Rol_ID == Inspector_Ordenamiento)
                    {
                        Tramites.P_Estatus_Persona_Inspecciona = Cls_Sessiones.Empleado_ID;
                    }
                }
                Tramites.P_Tipo_DataTable = "BANDEJA_TRAMITES";
                Tramites.P_Estatus = Cmb_Buscar_Solicitudes_Estatus.SelectedItem.Value;
                Tabla = Tramites.Consultar_DataTable();
            }

            //  validacion para ordenar la consulta no perteneciente a ordenamiento
            if (Estatus_Dependencia == false)
            {
                //  se ordenara la tabla por fecha
                DataView Dv_Ordenar = new DataView(Tabla);
                Dv_Ordenar.Sort = "Fecha asc, tramite asc, ESTATUS asc";
                Tabla = Dv_Ordenar.ToTable();
            }

            else
            {
                //  se ordenara la tabla por consecutivo uso de ordenamiento
                DataView Dv_Ordenar = new DataView(Tabla);
                Dv_Ordenar.Sort = " consecutivo , complemento";
                Tabla = Dv_Ordenar.ToTable();
            }

            // Se obtienen las Solicitudes de Tramite en proceso y pendientes.
            if (Tabla != null)
            {
                if (Tabla is DataTable)
                {
                    if (Tabla.Rows.Count > 0)
                        Session["GRID_BANDEJA_TRAMITES"] = Tabla;
                }
            }
            else
            {
                if (Cmb_Buscar_Solicitudes_Estatus.SelectedItem.Text == "PENDIENTE_PROCESO")
                    Lbl_Mensaje_Error.Text = "No hay Solicitudes de Tramite en la Bandeja para el Estatus 'PENDIENTE' o 'PROCESO' ";
                else
                    Lbl_Mensaje_Error.Text = "No hay Solicitudes de Tramite en la Bandeja para el Estatus '" + Cmb_Buscar_Solicitudes_Estatus.SelectedItem.Text + "' ";
                
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
            Llenar_Grid_Solicitudes_Tramites(0);
        }
        catch (Exception ex)
        {
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
                    //if (Cmb_Dependencias.Items.FindByValue(Dependencia_ID) != null)
                    //{
                    //    Cmb_Dependencias.SelectedValue = Dependencia_ID;
                    //}
                }
                catch (Exception Ex)
                {
                    throw new Exception(Ex.Message.ToString());
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
    ///NOMBRE DE LA FUNCIÓN: Btn_Solicitud_Click
    ///DESCRIPCIÓN: cargara la informacion de la solicitud
    ///PARAMETROS: 
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  25/junio/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Solicitud_Click(object sender, EventArgs e)
    {
        Cls_Rpt_Ven_Consultar_Tramites_Negocio Rs_Consulta = new Cls_Rpt_Ven_Consultar_Tramites_Negocio();
        Cls_Ope_Bandeja_Tramites_Negocio Solicitud = new Cls_Ope_Bandeja_Tramites_Negocio();
        DataTable Dt_Consulta = new DataTable();
        DataTable Dt_Historial_Actividades = new DataTable();
        try
        {
            ImageButton ImageButton = (ImageButton)sender;
            System.Web.UI.WebControls.TableCell TableCell = (System.Web.UI.WebControls.TableCell)ImageButton.Parent;
            GridViewRow Row = (GridViewRow)TableCell.Parent;
            Grid_Bandeja_Entrada.SelectedIndex = Row.RowIndex;
            int Fila = Row.RowIndex;

            Limpiar_Formulario();
            Grid_Bandeja_Entrada_SelectedIndexChanged(sender, null);
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al seleccionar una solicitud. Error: [" + Ex.Message + "]");
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Solicitudes_Tramites
    ///DESCRIPCIÓN: Llena el Grid de Solicitudes de Tramites.
    ///PARAMETROS:
    ///             1. Pagina.  Pagina de registros en que se mostrará el Grid.
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 14/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************      
    private void Llenar_Grid_Solicitudes_Tramites(Int32 Pagina)
    {
        Cls_Cat_Ort_Parametros_Negocio Obj_Parametros = new Cls_Cat_Ort_Parametros_Negocio();
        string Dependencia_ID_Ordenamiento = "";
        string Dependencia_ID_Ambiental = "";
        string Dependencia_ID_Urbanistico = "";
        string Dependencia_ID_Inmobiliario = "";
        string Dependencia_ID_Catastro = "";

        try
        {
            Obj_Parametros.Consultar_Parametros();

            //  para las dependencias de ordenamiento
            if (!string.IsNullOrEmpty(Obj_Parametros.P_Dependencia_ID_Ordenamiento))
                Dependencia_ID_Ordenamiento = Obj_Parametros.P_Dependencia_ID_Ordenamiento;

            if (!string.IsNullOrEmpty(Obj_Parametros.P_Dependencia_ID_Ambiental))
                Dependencia_ID_Ambiental = Obj_Parametros.P_Dependencia_ID_Ambiental;

            if (!string.IsNullOrEmpty(Obj_Parametros.P_Dependencia_ID_Urbanistico))
                Dependencia_ID_Urbanistico = Obj_Parametros.P_Dependencia_ID_Urbanistico;

            if (!string.IsNullOrEmpty(Obj_Parametros.P_Dependencia_ID_Inmobiliario))
                Dependencia_ID_Inmobiliario = Obj_Parametros.P_Dependencia_ID_Inmobiliario;

            if (!string.IsNullOrEmpty(Obj_Parametros.P_Dependencia_ID_Catastro))
                Dependencia_ID_Catastro = Obj_Parametros.P_Dependencia_ID_Catastro;

            Grid_Bandeja_Entrada.Columns[1].Visible = true;
            Grid_Bandeja_Entrada.Columns[2].Visible = true;
            Grid_Bandeja_Entrada.Columns[6].Visible = true;
            Grid_Bandeja_Entrada.Columns[9].Visible = true;
            
            if (Session["GRID_BANDEJA_TRAMITES"] != null)
                Grid_Bandeja_Entrada.DataSource = (DataTable)Session["GRID_BANDEJA_TRAMITES"];
         
            else
                Grid_Bandeja_Entrada.DataSource = new DataTable();
            
            
            Grid_Bandeja_Entrada.DataBind();

            if (Cls_Sessiones.Dependencia_ID_Empleado == Dependencia_ID_Ordenamiento   || Cls_Sessiones.Dependencia_ID_Empleado == Dependencia_ID_Ambiental
                    || Cls_Sessiones.Dependencia_ID_Empleado == Dependencia_ID_Inmobiliario  || Cls_Sessiones.Dependencia_ID_Empleado == Dependencia_ID_Urbanistico)
                Grid_Bandeja_Entrada.Columns[2].Visible = false;
           
            else
                Grid_Bandeja_Entrada.Columns[3].Visible = false;
            

            Grid_Bandeja_Entrada.Columns[1].Visible = false;
            Grid_Bandeja_Entrada.Columns[5].Visible = true;
            Grid_Bandeja_Entrada.Columns[9].Visible = false;
            Grid_Bandeja_Entrada.SelectedIndex = (-1);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }

    #endregion

    #region Grid Datos Solicitud

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Datos_Solicitud
    ///DESCRIPCIÓN: Llena el Grid de Datos de la Solicitud.
    ///PARAMETROS:
    ///             1.  Tabla.  DataTable con los datos con los que se va a llenar el Grid.
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 15/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************      
    private void Llenar_Grid_Datos_Solicitud(DataTable Tabla)
    {
        try
        {

            //  se ordenara la tabla por fecha
            DataView Dv_Ordenar = new DataView(Tabla);
            Dv_Ordenar.Sort = Cat_Tra_Datos_Tramite.Campo_Dato_ID;//SOLICITO asc, 
            DataTable Dt_Datos_Ordenados = Dv_Ordenar.ToTable();

            Grid_Datos_Tramite.SelectedIndex = (-1);
            Grid_Datos_Tramite.Columns[0].Visible = true;
            Grid_Datos_Tramite.Columns[1].Visible = true;

            if (Dt_Datos_Ordenados != null)
                Grid_Datos_Tramite.DataSource = Dt_Datos_Ordenados;
            
            else
                Grid_Datos_Tramite.DataSource = new DataTable();
            
            Grid_Datos_Tramite.DataBind();
            Grid_Datos_Tramite.Columns[0].Visible = false;
            Grid_Datos_Tramite.Columns[1].Visible = false;

            //Se cargará el valor del Dato
            if (Grid_Datos_Tramite.Rows.Count == Dt_Datos_Ordenados.Rows.Count)
            {
                for (Int32 Contador = 0; Contador < Grid_Datos_Tramite.Rows.Count; Contador++)
                {
                    TextBox Txt_Valor_Temporal = (TextBox)Grid_Datos_Tramite.Rows[Contador].Cells[3].Controls[1];
                    Txt_Valor_Temporal.Text = Dt_Datos_Ordenados.Rows[Contador][3].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }

    #endregion

    #region Grid Documentacion Solicitud

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Documentacion_Solicitud
    ///DESCRIPCIÓN: Llena el Grid de Documentacion de Solicitud.
    ///PARAMETROS:
    ///             1.  Tabla.  DataTable con los datos con los que se va a llenar el Grid.
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 15/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************      
    private void Llenar_Grid_Documentacion_Solicitud(DataTable Tabla)
    {
        try
        {
            Grid_Documentos_Tramite.SelectedIndex = (-1);
            Grid_Documentos_Tramite.Columns[0].Visible = true;
            Grid_Documentos_Tramite.Columns[1].Visible = true;
            Grid_Documentos_Tramite.Columns[3].Visible = true;
            
            if (Tabla != null)
                Grid_Documentos_Tramite.DataSource = Tabla;
            
            else
                Grid_Documentos_Tramite.DataSource = new DataTable();
            
            Grid_Documentos_Tramite.DataBind();
            Grid_Documentos_Tramite.Columns[0].Visible = false;
            Grid_Documentos_Tramite.Columns[1].Visible = false;
            Grid_Documentos_Tramite.Columns[3].Visible = false;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
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
            if (System.IO.File.Exists(Server.MapPath(Ruta)))
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Window_Archivo_Archivos", "window.open('" + Ruta + "','Window_Archivo','left=0,top=0')", true);
            
            else
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "nada", "alert('El archivo no existe o fue eliminado');", true);
            
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "nada", "alert('" + ex.Message + "');", true);
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
    private void Mensaje_Error(Boolean Habilitar, String Texto_Error)
    {
        try
        {
            Div_Contenedor_Msj_Error.Visible = Habilitar;
            Lbl_Mensaje_Error.Visible = Habilitar;
            Lbl_Mensaje_Error.Text = Texto_Error;
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.ToString();
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Mostrar_Filtro_Fecha
    ///DESCRIPCIÓN: Muestra el panel de los filtros de las fechas
    ///PARAMETROS:
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 19/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    public void Mostrar_Filtro_Fecha()
    {
        Cls_Cat_Ort_Parametros_Negocio Obj_Parametros = new Cls_Cat_Ort_Parametros_Negocio();
        string Director_Ordenamiento = "";
        string Director_Ambiental = "";
        string Director_Urbanistico = "";
        string Director_Fraccionamientos = "";
        try
        {
            Obj_Parametros.Consultar_Parametros();

            //  para los roles de los directores de ordenamiento
            if (!string.IsNullOrEmpty(Obj_Parametros.P_Rol_Director_Ordenamiento))
                Director_Ordenamiento = Obj_Parametros.P_Rol_Director_Ordenamiento;

            if (!string.IsNullOrEmpty(Obj_Parametros.P_Rol_Director_Ambiental))
                Director_Ambiental = Obj_Parametros.P_Rol_Director_Ambiental;

            if (!string.IsNullOrEmpty(Obj_Parametros.P_Rol_Director_Fraccionamientos))
                Director_Fraccionamientos = Obj_Parametros.P_Rol_Director_Fraccionamientos;

            if (!string.IsNullOrEmpty(Obj_Parametros.P_Rol_Director_Urbana))
                Director_Urbanistico = Obj_Parametros.P_Rol_Director_Urbana;

            if (Cls_Sessiones.Rol_ID == Director_Ordenamiento ||
                    Cls_Sessiones.Rol_ID == Director_Ambiental ||
                    Cls_Sessiones.Rol_ID == Director_Fraccionamientos ||
                    Cls_Sessiones.Rol_ID == Director_Urbanistico)
            {
                //  se oculta el filtro de fechas
                Pnl_Filtro_Fechas.Style.Value = "width:98%; display:block";
            }
            else
                Pnl_Filtro_Fechas.Style.Value = "width:98%; display:none";
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "nada", "alert('" + ex.Message + "');", true);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Mostrar_Archivo_Word
    ///DESCRIPCIÓN: Muestra un Archivo del cual se le pasa la ruta como parametro.
    ///PARAMETROS:
    ///             1.  Ruta.  Ruta del Archivo.
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 19/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    public void Mostrar_Archivo_Word(String Ruta)
    {
        try
        {
            if (System.IO.File.Exists(Server.MapPath(Ruta)))
            {
                String Pagina = "../Nomina/Frm_Mostrar_Archivos.aspx?Documento=" + Ruta;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Window_Archivo_Archivos", "window.open('" + Pagina + "','Window_Archivo','left=0,top=0')", true);
            }
            else
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "nada", "alert('El archivo no existe o fue eliminado');", true);
            
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "nada", "alert('" + ex.Message + "');", true);
        }
    }

    #endregion

    #region Grid Documentos Seguimiento

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Cargar_Combo_Nombres
    ///DESCRIPCIÓN          : se cargara las areas
    ///PARAMETROS           : 
    ///CREO                 : Hugo Enrique Ramírez Aguilera
    //FECHA_CREO           : 10/Octubre/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Cargar_Combo_Nombres()
    {
        Cls_Ope_Ort_Bitacora_Documentos_Negocio Negocio_Consultar_Solicitud = new Cls_Ope_Ort_Bitacora_Documentos_Negocio();
        DataTable Dt_Consulta = new DataTable();
        try
        {
            //Negocio_Consultar_Solicitud.P_Nombre_Usuario = "ordenamiento territorial";
            Dt_Consulta = Negocio_Consultar_Solicitud.Consultar_Personal();
            if (Dt_Consulta != null && Dt_Consulta.Rows.Count > 0)
            {
                Cmb_Inspector.DataSource = Dt_Consulta;
                Cmb_Inspector.DataValueField = Cat_Empleados.Campo_Empleado_ID;
                Cmb_Inspector.DataTextField = "Nombre_Usuario";
                Cmb_Inspector.DataBind();
                Cmb_Inspector.Items.Insert(0, "< SELECCIONE >");
            }
            else
            {
                Cmb_Inspector.DataSource = new DataTable();
                Cmb_Inspector.DataBind();

                Lbl_Mensaje_Error.Text = "No se encuentran personal";
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cargar_Documentos_Seguimiento
    ///DESCRIPCIÓN: Llena el Grid de Documentos de Seguimiento del  Subproceso.
    ///PARAMETROS:
    ///             1.  Solicitud.  Objeto del cual se obtienen los datos para cargar
    ///                             el Grid.
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 20/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    private void Cargar_Documentos_Seguimiento(Cls_Ope_Bandeja_Tramites_Negocio Solicitud, String Rol_Director_Ordenamiento, String Rol_Director_Ambiental
                                                , String Rol_Director_Fraccionamientos, String Rol_Director_Urbano)
    {
        Boolean Estado_Columna_Grid = false;
        try
        {
             if (Rol_Director_Ordenamiento == Cls_Sessiones.Rol_ID ||
                    Rol_Director_Ambiental == Cls_Sessiones.Rol_ID ||
                    Rol_Director_Fraccionamientos == Cls_Sessiones.Rol_ID ||
                    Rol_Director_Urbano == Cls_Sessiones.Rol_ID)
                Estado_Columna_Grid = true;

            else
                Estado_Columna_Grid = false;

            if (Directory.Exists(MapPath("../../Archivos/" + Solicitud.P_Clave_Solicitud.Trim() + "/")))
            {
                DataTable Documentos_Seguimiento = null;
                String[] Archivos = Directory.GetFiles(MapPath("../../Archivos/" + Solicitud.P_Clave_Solicitud.Trim() + "/"));
                for (Int32 Contador = 0; Contador < Archivos.Length; Contador++)
                {
                    Boolean Encontrado = false;
                    String Documento = Path.GetFileName(Archivos[Contador].Trim());
                    for (Int32 Contador2 = 0; Contador2 < Solicitud.P_Documentos_Solicitud.Rows.Count; Contador2++)
                    {
                        String Documento_Tramite = Path.GetFileName(Solicitud.P_Documentos_Solicitud.Rows[Contador2][3].ToString().Trim());
                        if (Documento.Equals(Documento_Tramite))
                        {
                            Encontrado = true;
                            break;
                        }
                    }
                    if (!Encontrado)
                    {
                        if (Documentos_Seguimiento == null)
                        {
                            Documentos_Seguimiento = new DataTable();
                            Documentos_Seguimiento.Columns.Add("NOMBRE_DOCUMENTO", Type.GetType("System.String"));
                            Documentos_Seguimiento.Columns.Add("URL", Type.GetType("System.String"));
                        }
                        DataRow Fila = Documentos_Seguimiento.NewRow();
                        Fila["NOMBRE_DOCUMENTO"] = Documento;
                        Fila["URL"] = Archivos[Contador].Trim();
                        Documentos_Seguimiento.Rows.Add(Fila);
                    }
                }
                if (Documentos_Seguimiento == null)
                {
                    Llenar_Grid_Documentos_Seguimiento(new DataTable(), Estado_Columna_Grid);
                }
                else
                {
                    Llenar_Grid_Documentos_Seguimiento(Documentos_Seguimiento, Estado_Columna_Grid);
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "nada", "alert('" + ex.Message + "')", true);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Documentos_Seguimiento
    ///DESCRIPCIÓN: Llena el Grid de Platillas del  Subproceso.
    ///PARAMETROS:
    ///             1.  Tabla.  DataTable con los datos con los que se va a llenar el Grid.
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 20/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************      
    private void Llenar_Grid_Documentos_Seguimiento(DataTable Tabla, Boolean Estado_Columna_Grid)
    {
        try
        {
            Grid_Documentos_Seguimiento.Columns[1].Visible = true;
            Grid_Documentos_Seguimiento.Columns[3].Visible = true;
            Grid_Documentos_Seguimiento.SelectedIndex = (-1);

            if (Tabla != null)
                Grid_Documentos_Seguimiento.DataSource = Tabla;
           
            else
                Grid_Documentos_Seguimiento.DataSource = new DataTable();
            
            Grid_Documentos_Seguimiento.DataBind();
            Grid_Documentos_Seguimiento.Columns[1].Visible = false;

            //if(Estado_Columna_Grid == false)
            //    Grid_Documentos_Seguimiento.Columns[3].Visible = false;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Mostrar_Componenetes_Evaluar
    ///DESCRIPCIÓN          : se habilitan los elementos para evaluar
    ///PARAMETROS           : 
    ///CREO                 : Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO           : 17/Septiembre/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    private void Mostrar_Componenetes_Evaluar(Boolean Accion)
    {
        try
        {
            if (Accion == true)
            {
                Lbl_Evaluacion.Style.Value = "display:block";
                Cmb_Evaluacion.Style.Value = "display:block";
                Lbl_Evaluacion.Visible = true;
                Cmb_Evaluacion.Visible = true;
            }
            else
            {
                Lbl_Evaluacion.Style.Value = "display:none";
                Cmb_Evaluacion.Style.Value = "display:none";
                Lbl_Evaluacion.Visible = false;
                Cmb_Evaluacion.Visible = false;
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Mostrar_Mensaje_Error " + ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Obtener_Nombre_Archivo
    ///DESCRIPCIÓN:          Se obtendra el nombre del archivo sin su extension
    ///PARAMETROS:           String Ruta, direccion que 
    ///                         contiene el nombre del archivo al cual se le sacara la extension
    ///CREO:                 Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:           29/Mayo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    private string Obtener_Extension(String Archivo)
    {

        String Nombre_Archivo = "";
        try
        {
            int Incide = Archivo.LastIndexOf(".");
            for (int Contador_Archivo = 0; Contador_Archivo < Incide; Contador_Archivo++)
            {
                Nombre_Archivo += Archivo.Substring(Contador_Archivo, 1);
            }

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
        return Nombre_Archivo;
    }

    #endregion

    #region Grid Platillas

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Platillas_Subproceso
    ///DESCRIPCIÓN: Llena el Grid de Platillas del  Subproceso.
    ///PARAMETROS:
    ///             1.  Tabla.  DataTable con los datos con los que se va a llenar el Grid.
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 16/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************      
    private void Llenar_Grid_Platillas_Subproceso(DataTable Tabla)
    {
        try
        {
            Grid_Plantillas.Columns[1].Visible = true;
            Grid_Plantillas.Columns[3].Visible = true;
            Grid_Plantillas.SelectedIndex = (-1);
            
            if (Tabla != null)
                Grid_Plantillas.DataSource = Tabla;
            
            else
                Grid_Plantillas.DataSource = new DataTable();
            
            Grid_Plantillas.DataBind();
            Grid_Plantillas.Columns[1].Visible = false;
            Grid_Plantillas.Columns[3].Visible = false;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE:      Llenar_Grid_Formatos_Subproceso
    ///DESCRIPCIÓN: Llena el Grid de formatos del  Subproceso.
    ///PARAMETROS:  1.  Tabla.  DataTable con los datos con los que se va a llenar el Grid.
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  28/Mayo/2012 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************      
    private void Llenar_Grid_Formatos_Subproceso(DataTable Tabla)
    {
        try
        {
            Grid_Detalles_Formatos.Columns[1].Visible = true;
            Grid_Detalles_Formatos.Columns[3].Visible = true;
            Grid_Detalles_Formatos.SelectedIndex = (-1);
            
            if (Tabla != null)
                Grid_Detalles_Formatos.DataSource = Tabla;
            
            else
                Grid_Detalles_Formatos.DataSource = new DataTable();
           
            Grid_Detalles_Formatos.DataBind();
            Grid_Detalles_Formatos.Columns[1].Visible = false;
            Grid_Detalles_Formatos.Columns[3].Visible = false;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }

    #endregion

    #region Grid Plantilla Marcadores

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Marcadores_Platillas
    ///DESCRIPCIÓN: Llena el Grid de Marcadores Platillas.
    ///PARAMETROS:
    ///             1.  Tabla.  DataTable con los datos con los que se va a llenar el Grid.
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 18/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************      
    private void Llenar_Grid_Marcadores_Platillas(DataTable Tabla)
    {
        try
        {
            Grid_Marcadores_Platilla.Columns[0].Visible = true;
            Grid_Marcadores_Platilla.SelectedIndex = (-1);
            
            if (Tabla != null)
                Grid_Marcadores_Platilla.DataSource = Tabla;
            
            else
                Grid_Marcadores_Platilla.DataSource = new DataTable();
            
            Grid_Marcadores_Platilla.DataBind();
            Grid_Marcadores_Platilla.Columns[0].Visible = false;
            Buscar_Marcadores_Fuente_Datos();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cargar_Marcadores_Planilla
    ///DESCRIPCIÓN: Carga los Marcadores de la plantilla en la Tabla.
    ///PARAMETROS:  1.  Nombre_Plantilla.   Nombre de la Platilla que se va a leer para
    ///                                     llenar el Grid.
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 18/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Cargar_Marcadores_Planilla(String Nombre_Plantilla)
    {
        Cls_Interaccion_Word Plantilla = new Cls_Interaccion_Word();
        try
        {
            Plantilla.P_Documento_Origen = MapPath("../../Plantillas_Word/" + Nombre_Plantilla);
            Plantilla.Iniciar_Aplicacion();
            DataTable Tabla_Marcadores = Plantilla.Obtener_Marcadores();
            Plantilla.Cerrar_Aplicacion();
            Llenar_Grid_Marcadores_Platillas(Tabla_Marcadores);
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Text = "Error Cargar_Marcadores_Planilla";
            Lbl_Mensaje_Error.Text += ex.Message.ToString();
            Div_Contenedor_Msj_Error.Visible = true;
            throw new Exception(ex.Message.ToString());
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Consultar_Propietario
    /// DESCRIPCIÓN: Regresar el nombre del propietario de la cuenta predial con el id proporcionado
    /// PARÁMETROS:
    /// 		1. Cuenta_Predial_ID: id de la cuenta predial a consultar
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 10-jun-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private String Consultar_Propietario(String Cuenta_Predial_ID)
    {
        String Propietario = "";
        DataTable Dt_Resultado_Consulta;
        var Consulta_Propietario_Negocio = new Cls_Ope_Pre_Pagos_Instituciones_Externas_Negocio();

        try
        {
            if (!String.IsNullOrEmpty(Cuenta_Predial_ID))
            {
                // consultar cuenta predial
                Consulta_Propietario_Negocio.P_Cuenta_Predial_Id = Cuenta_Predial_ID;
                Dt_Resultado_Consulta = Consulta_Propietario_Negocio.Consultar_Propietario();
                
                if (Dt_Resultado_Consulta != null && Dt_Resultado_Consulta.Rows.Count > 0)
                    Propietario = Dt_Resultado_Consulta.Rows[0]["NOMBRE_PROPIETARIO"].ToString();
               
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            IBtn_Imagen_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message;
        }
        return Propietario;
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Consultar_Cuenta_Predial_ID
    /// DESCRIPCIÓN: Regresar el id de la cuenta predial, se busca mediante la cuenta predial 
    /// PARÁMETROS:
    /// 		1. Cuenta_Predial: cuenta predial a localizar
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 10-jun-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private String Consultar_Cuenta_Predial_ID(String Cuenta_Predial)
    {
        var Consulta_Cuenta = new Cls_Ope_Pre_Pagos_Instituciones_Externas_Negocio();
        String Cuenta_Predial_ID = "";
        DataTable Dt_Resultado_Consulta;

        try
        {
            if (!String.IsNullOrEmpty(Cuenta_Predial))
            {
                // consultar cuenta predial
                Consulta_Cuenta.P_Cuenta_Predial = Cuenta_Predial;
                Dt_Resultado_Consulta = Consulta_Cuenta.Consultar_Cuenta_Predial_ID();
                
                if (Dt_Resultado_Consulta != null && Dt_Resultado_Consulta.Rows.Count > 0)
                    Cuenta_Predial_ID = Dt_Resultado_Consulta.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID].ToString();
               
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Consultar_Cuenta_Predial_ID: " + ex.Message.ToString(), ex);
        }
        return Cuenta_Predial_ID;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Buscar_Marcadores_Fuente_Datos
    ///DESCRIPCIÓN: Busca coincidencias entre los marcadores de la Tabla y los de la fuente
    ///             de datos.
    ///PARAMETROS:
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 23/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Buscar_Marcadores_Fuente_Datos()
    {
        try
        {
            if (Grid_Marcadores_Platilla.Rows.Count > 0 && Session["Dt_Fuente_Datos"] != null)
            {
                DataTable Fuente_Datos = (DataTable)Session["Dt_Fuente_Datos"];
                for (Int32 Contador = 0; Contador < Grid_Marcadores_Platilla.Rows.Count; Contador++)
                {
                    String Marcador = Grid_Marcadores_Platilla.Rows[Contador].Cells[0].Text.ToUpper();
                    for (Int32 Contador_DT = 0; Contador_DT < Fuente_Datos.Columns.Count; Contador_DT++)
                    {
                        String Elemento_Fuente = Fuente_Datos.Columns[Contador_DT].ColumnName.ToUpper();
                        if (Elemento_Fuente.Equals(Marcador))
                        {
                            TextBox Text_Temporal = (TextBox)Grid_Marcadores_Platilla.Rows[Contador].FindControl("Txt_Valor_Marcador");
                            
                            if (Fuente_Datos.Columns[Contador_DT].DataType == Type.GetType("System.DateTime"))
                                Text_Temporal.Text = ((DateTime)Fuente_Datos.Rows[0][Contador_DT]).ToString("dd/MMM/yyyy");
                           
                            else
                                Text_Temporal.Text = Fuente_Datos.Rows[0][Contador_DT].ToString();
                            
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }

    #endregion

    #region Generales

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Limpiar_Formulario
    ///DESCRIPCIÓN: Se limpian los controles del Formulario.
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 15/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///******************************************************************************* 
    private void Limpiar_Formulario()
    {
        try
        {
            //  campos ocultos
            Hdf_Solicitud_ID.Value = "";
            Hdf_Subproceso_ID.Value = "";
            Hdf_Plantilla_Seleccionada.Value = "";
            Hdf_Condicion_Si.Value = "";
            Hdf_Condicion_No.Value = "";
            Hdf_Tramite_Id.Value = "";
            Hdf_Costo_Total.Value = "";
            Hdf_Contribuyente_Id.Value = "";
            //  cajas de texto
            Txt_Clave_Solicitud.Text = "";
            Txt_Porcentaje_Avance.Text = "";
            Txt_Porcentaje_Actual_Proceso.Text = "";
            Txt_Nombre_Tramite.Text = "";
            Txt_Solicito.Text = "";
            Txt_Subproceso.Text = "";
            Txt_Estatus.Text = "";
            Txt_Fecha_Solicitud.Text = "";
            Txt_Comentarios_Internos.Text = "";
            Txt_Comentarios_Evaluacion.Text = "";
            Txt_Cuenta_Predial.Text = "";
            Txt_Propietario_Cuenta_Predial.Text = "";
            Txt_Costo_Total.Text = "";
            Txt_Otros_Predio.Text = "";

            //datos adicionales
            Txt_Propietario_Cuenta_Predial.Text = "";
            Txt_Direccion_Predio.Text = "";
            Txt_Calle_Predio.Text = "";
            Txt_Numero_Predio.Text = "";
            Txt_Manzana_Predio.Text = "";
            Txt_Lote_Predio.Text = "";

            //  combos
            Cmb_Evaluacion.SelectedIndex = 0;
            Cmb_Zonas.SelectedIndex = -1;
            Cmb_Supervisor_Zona.SelectedIndex = -1;
            Cmb_Condicion.SelectedIndex = -1;
            //  grids
            Grid_Datos_Tramite.DataSource = new DataTable();
            Grid_Datos_Tramite.DataBind();
            Grid_Documentos_Tramite.DataSource = new DataTable();
            Grid_Documentos_Tramite.DataBind();
            Grid_Plantillas.DataSource = new DataTable();
            Grid_Plantillas.DataBind();
            Grid_Marcadores_Platilla.DataSource = new DataTable();
            Grid_Marcadores_Platilla.DataBind();
            MPE_Crear_Plantilla.TargetControlID = Btn_Comodin_FGC.ID;
            Session.Remove("Dt_Fuente_Datos");
            Session.Remove("Opinion_Solicitud_Id");
            Session.Remove("Cedula_Solicitud_Id");

            Tbl_Fechas_Vigencia.Visible = false;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Configuracion_Catalogo
    ///DESCRIPCIÓN: Maneja la habilitacion e inhabilitacion de los componentes.
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 16/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///******************************************************************************* 
    private void Configuracion_Catalogo(Boolean Estatus)
    {
        try
        {
            Grid_Datos_Tramite.Enabled = Estatus;
            Grid_Documentos_Tramite.Enabled = Estatus;
            Grid_Plantillas.Enabled = Estatus;
            Cmb_Evaluacion.Enabled = Estatus;
            Btn_Evaluar.Enabled = Estatus;
            Btn_Copiar.Visible = false;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Habilitar_Comentarios
    ///DESCRIPCIÓN: habilita los comentarios
    ///PARAMETROS:     
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  16/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///******************************************************************************* 
    private void Habilitar_Comentarios(Boolean Estatus)
    {
        try
        {
            Txt_Comentarios_Evaluacion.Enabled = Estatus;
            Txt_Comentarios_Internos.Enabled = Estatus;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Habilitar_Boton_Modificar_Zona
    ///DESCRIPCIÓN: habilita el link
    ///PARAMETROS:     
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  21/Septiembre/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///******************************************************************************* 
    private void Habilitar_Boton_Modificar_Zona()
    {
        //string Director_Ordenamiento = "";
        //string Director_Ambiental = "";
        //string Director_Urbanistico = "";
        //string Director_Fraccionamientos = "";
        //Cls_Cat_Ort_Parametros_Negocio Obj_Parametros = new Cls_Cat_Ort_Parametros_Negocio();
        //try
        //{
            //    Obj_Parametros.Consultar_Parametros();

            //    //  se obtienen los roles de los directores de ordenamiento
            //    if (!string.IsNullOrEmpty(Obj_Parametros.P_Rol_Director_Ordenamiento))
            //        Director_Ordenamiento = Obj_Parametros.P_Rol_Director_Ordenamiento;

            //    if (!string.IsNullOrEmpty(Obj_Parametros.P_Rol_Director_Ambiental))
            //        Director_Ambiental = Obj_Parametros.P_Rol_Director_Ambiental;

            //    if (!string.IsNullOrEmpty(Obj_Parametros.P_Rol_Director_Fraccionamientos))
            //        Director_Fraccionamientos = Obj_Parametros.P_Rol_Director_Fraccionamientos;

            //    if (!string.IsNullOrEmpty(Obj_Parametros.P_Rol_Director_Urbana))
            //        Director_Urbanistico = Obj_Parametros.P_Rol_Director_Urbana;

            //    if (Cls_Sessiones.Rol_ID == Director_Ordenamiento ||
            //            Cls_Sessiones.Rol_ID == Director_Ambiental ||
            //            Cls_Sessiones.Rol_ID == Director_Fraccionamientos ||
            //            Cls_Sessiones.Rol_ID == Director_Urbanistico)
            //    {
            Btn_Modificar.Enabled = true;
            Btn_Link_Cedula_Visita.Style.Value = "display: block";
            Btn_Link_Opiniones.Style.Value = "display: block";
            //}
            //else
            //{
            //    Btn_Modificar.Enabled = false;
            //    Btn_Link_Cedula_Visita.Style.Value = "display: none";
            //    Btn_Link_Opiniones.Style.Value = "display: none";
            //}
        //}
        //catch (Exception ex)
        //{
        //    throw new Exception(ex.Message.ToString());
        //}
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Habilitar_Comentarios
    ///DESCRIPCIÓN: habilita los comentarios
    ///PARAMETROS:     
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  10/Sempiembre/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///******************************************************************************* 
    private void Habilitar_Elementos_Revision_Ficha(Boolean Estatus)
    {
        try
        {
            Cmb_Zonas.Enabled = Estatus;
            Cmb_Supervisor_Zona.Enabled = Estatus;
            Cmb_Condicion.Enabled = Estatus;
            Txt_Comentarios_Internos.Enabled = Estatus;
            Txt_Comentarios_Evaluacion.Enabled = Estatus;
            Btn_Evaluar.Visible = Estatus;
            Btn_Cancelar_Solicitud.Visible = Estatus;
            Btn_Guardar_Datos_Dictamen.Visible = Estatus;
            AFU_Subir_Archivo.Visible = Estatus;
            Btn_Actualizar_Documentos.Visible = Estatus;
            Grid_Plantillas.Enabled = Estatus;
            Grid_Detalles_Formatos.Enabled = Estatus;
            Cmb_Buscar_Solicitudes_Estatus.Visible = Estatus;
            Btn_Buscar_Solicitudes_Estatus.Visible = Estatus;
            Grid_Documentos_Tramite.Enabled = !Estatus;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Calcular_Costo
    ///DESCRIPCIÓN: habilita los comentarios
    ///PARAMETROS:     
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  10/Sempiembre/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///******************************************************************************* 
    private void Calcular_Costo(String Actividad_ID, Cls_Ope_Bandeja_Tramites_Negocio Neg_Solicitud)
    {
        DataTable Dt_Subproceso;
        DataTable Dt_Solicitud_Hijas = new DataTable();
        String Porcentaje_Penalizacion = "";
        Cls_Cat_Tramites_Negocio Neg_Consulta_Subprocesos = new Cls_Cat_Tramites_Negocio(); 
        Cls_Cat_Ort_Parametros_Negocio Obj_Parametros = new Cls_Cat_Ort_Parametros_Negocio();
        Double Resultado = 0.0;
        Double Resultado_Solicitud_Hija = 0.0;
        Double Suma_Costos = 0.0;
        Double Costo_Bitacora = 0.0;
        Double Costo_Perito = 0.0;

        try
        {
            //  se consultan los parametros
            Obj_Parametros.Consultar_Parametros();

            if (!String.IsNullOrEmpty(Obj_Parametros.P_Costo_Bitacora))
                Costo_Bitacora = Convert.ToDouble(Obj_Parametros.P_Costo_Bitacora);

            if (!String.IsNullOrEmpty(Obj_Parametros.P_Costo_Perito))
                Costo_Perito = Convert.ToDouble(Obj_Parametros.P_Costo_Perito);



            //  si contiene solicitudes hijas se obtendra la suma de estas
            Dt_Solicitud_Hijas = Neg_Solicitud.Consultar_Solicitudes_Hija();
            Porcentaje_Penalizacion = Neg_Solicitud.Consultar_Penalizaciones();

            if (Dt_Solicitud_Hijas is DataTable)
            {
                if (Dt_Solicitud_Hijas != null && Dt_Solicitud_Hijas.Rows.Count > 0)
                {
                    foreach (DataRow Registro in Dt_Solicitud_Hijas.Rows)
                    {
                        if (Registro is DataRow)
                        {
                            if (!String.IsNullOrEmpty(Registro[Ope_Tra_Solicitud.Campo_Costo_Total].ToString()))
                            {
                                Resultado_Solicitud_Hija += Convert.ToDouble(Registro[Ope_Tra_Solicitud.Campo_Costo_Total].ToString());
                            }
                        }
                    }
                }
            }


            //  proceso normal
            Neg_Consulta_Subprocesos.P_Sub_Proceso_ID = Actividad_ID;
            Dt_Subproceso = Neg_Consulta_Subprocesos.Consultar_Subprocesos_Tramite();

            // validar que la consulta haya regresado resultados
            if (Dt_Subproceso != null && Dt_Subproceso.Rows.Count > 0)
            {
                // dar de alta el pasivo para el pago si el subproceso siguiente es de tipo COBRO
                foreach (DataRow Fila_Subproceso in Dt_Subproceso.Rows)
                {
                    // datos del tramite
                    Cls_Cat_Tramites_Negocio Negocio_Datos_Tramite = new Cls_Cat_Tramites_Negocio();
                    Negocio_Datos_Tramite.P_Tramite_ID = Neg_Solicitud.P_Tramite_id;
                    Negocio_Datos_Tramite = Negocio_Datos_Tramite.Consultar_Datos_Tramite();

                    //  datos del ditamen
                    Cls_Ope_Bandeja_Tramites_Negocio Negocio_Datos_Dictamen = new Cls_Ope_Bandeja_Tramites_Negocio();
                    Negocio_Datos_Dictamen.P_Solicitud_ID = Neg_Solicitud.P_Solicitud_ID;
                    Negocio_Datos_Dictamen = Negocio_Datos_Dictamen.Consultar_Datos_Solicitud();
                    DataTable Dt_Consulta_Dato_Final_Llenado = Negocio_Datos_Dictamen.Consultar_Datos_Finales_Operacion();
                    DataTable Dt_Matriz_Costo = Negocio_Datos_Dictamen.Consultar_Costo_Matriz();

                    Double Parametro1 = 0.0;
                    Double Parametro2 = 0.0;
                    Double Costo_Matriz = 0.0;

                    int Cnt_Tipo_Matriz = 0;
                    String Tipo_Dato_Matriz = "";

                    if (!String.IsNullOrEmpty(Negocio_Datos_Tramite.P_Parametro1))
                    {
                        if (Dt_Consulta_Dato_Final_Llenado != null && Dt_Consulta_Dato_Final_Llenado.Rows.Count > 0)
                        {
                            foreach (DataRow Registro in Dt_Consulta_Dato_Final_Llenado.Rows)
                            {
                                if (Negocio_Datos_Tramite.P_Parametro1 == Registro["NOMBRE_DATO"].ToString())
                                {
                                    if (!String.IsNullOrEmpty(Registro["VALOR"].ToString()) && Registro["VALOR"].ToString() != "0")
                                    {
                                        Parametro1 = Convert.ToDouble(Registro["VALOR"].ToString());
                                        Cnt_Tipo_Matriz++;
                                    }
                                }

                                if (Registro["NOMBRE_DATO"].ToString().ToUpper().Trim() == "Tipo Matriz".ToUpper().Trim())
                                {
                                    if (!String.IsNullOrEmpty(Registro["VALOR"].ToString()) && Registro["VALOR"].ToString() != "0")
                                    {
                                        Tipo_Dato_Matriz = Registro["VALOR"].ToString();
                                        Cnt_Tipo_Matriz++;
                                    }
                                }

                                if (Cnt_Tipo_Matriz == 2)
                                {
                                    break;
                                }

                            }
                        }
                    }// fin del if

                    if (Tipo_Dato_Matriz != "")
                    {
                        if (Dt_Matriz_Costo != null && Dt_Matriz_Costo.Rows.Count > 0)
                        {
                            foreach (DataRow Registro in Dt_Matriz_Costo.Rows)
                            {
                                if (Registro[Ope_Tra_Matriz_Costo.Campo_Tipo].ToString().ToUpper().Trim() == Tipo_Dato_Matriz.Trim().ToUpper())
                                {
                                    Costo_Matriz = Convert.ToDouble(Registro[Ope_Tra_Matriz_Costo.Campo_Costo_Base].ToString());
                                    break;
                                }
                            }
                        }
                    }

                    if (!String.IsNullOrEmpty(Negocio_Datos_Tramite.P_Parametro2))
                    {
                        if (Dt_Consulta_Dato_Final_Llenado != null && Dt_Consulta_Dato_Final_Llenado.Rows.Count > 0)
                        {
                            foreach (DataRow Registro in Dt_Consulta_Dato_Final_Llenado.Rows)
                            {
                                if (Negocio_Datos_Tramite.P_Parametro2 == Registro["NOMBRE_DATO"].ToString())
                                {
                                    if (!String.IsNullOrEmpty(Registro["VALOR"].ToString()) && Registro["VALOR"].ToString() != "0")
                                    {
                                        Parametro2 = Convert.ToDouble(Registro["VALOR"].ToString());
                                        break;
                                    }
                                }

                            }
                        }
                    }// fin del if

                    if (Parametro1 != 0 && Parametro2 != 0)
                    {
                        if (Negocio_Datos_Tramite.P_Operador2 == "+")
                            Resultado = Parametro1 + Parametro2;

                        else if (Negocio_Datos_Tramite.P_Operador2 == "-")
                            Resultado = Parametro1 - Parametro2;

                        else if (Negocio_Datos_Tramite.P_Operador2 == "/")
                            Resultado = Parametro1 / Parametro2;

                        else if (Negocio_Datos_Tramite.P_Operador2 == "*")
                            Resultado = Parametro1 * Parametro2;
                    }

                    else if (Parametro1 != 0)
                        Resultado = Parametro1;

                    else if (Parametro2 != 0)
                        Resultado = Parametro2;


                    //  se asigna el costo de la matriz o del detalle de la solicitud
                    if (Resultado != 0.0)
                    {
                        if (Costo_Matriz == 0.0)
                            Costo_Matriz = Negocio_Datos_Tramite.P_Costo;

                        //  operador sera el primero que compara costo del [(tramite) {operador * - / +} (parametros)]
                        if (Negocio_Datos_Tramite.P_Operador1 == "+")
                            Resultado = Resultado + Costo_Matriz;

                        else if (Negocio_Datos_Tramite.P_Operador1 == "-")
                            Resultado = Resultado - Costo_Matriz;

                        else if (Negocio_Datos_Tramite.P_Operador1 == "/")
                            Resultado = Resultado / Costo_Matriz;

                        else if (Negocio_Datos_Tramite.P_Operador1 == "*")
                            Resultado = Resultado * Costo_Matriz;

                    }
                    else
                        Resultado = Convert.ToDouble(Neg_Solicitud.P_Costo_Total);
                }

                if (Porcentaje_Penalizacion != "")
                {
                    Suma_Costos = (Resultado_Solicitud_Hija + Resultado) * (1 + ((Convert.ToDouble(Porcentaje_Penalizacion)) / 100));
                }
                else
                {
                    Suma_Costos = (Resultado_Solicitud_Hija + Resultado);
                }

                if (!String.IsNullOrEmpty(Neg_Solicitud.P_Inspector_ID))
                {
                    Suma_Costos += Costo_Bitacora + Costo_Perito;
                }

                //  se suma el total de la solicitud con las solicitudes hijas
                Txt_Costo_Total.ToolTip = "Costo Total: $ " + Suma_Costos;
                Hdf_Costo_Total.Value = "" + Suma_Costos;

                if (Resultado != 0.0)
                    Txt_Costo_Total.Text = "" + Resultado;

            }

        }
        catch (Exception ex)
        {
            throw new Exception("Calcular_Costo " + ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Enviar_Correo
    ///DESCRIPCIÓN: Envia un correo al Usuario cuando el estatus de la solicitud cambio
    ///             a 'DETENIDO', 'CANDELADO' ó 'TERMINADO'.
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 20/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///******************************************************************************* 
    private void Enviar_Correo(Cls_Ope_Bandeja_Tramites_Negocio Solicitud)
    {
        Cls_Mail mail = new Cls_Mail();
        try
        {
            if (Solicitud.P_Correo_Electronico != null && Solicitud.P_Correo_Electronico.Trim().Length > 0)
            {
                //  para
                mail.P_Recibe = Solicitud.P_Correo_Electronico;
                //  de
                mail.P_Envia = Cls_Cat_Ate_Peticiones_Datos.Consulta_Parametros().Rows[0][Presidencia.Constantes.Apl_Parametros.Campo_Correo_Saliente].ToString();
                //  servidor
                mail.P_Servidor = Cls_Cat_Ate_Peticiones_Datos.Consulta_Parametros().Rows[0][Presidencia.Constantes.Apl_Parametros.Campo_Servidor_Correo].ToString();
                //   contraseña
                mail.P_Password = Cls_Cat_Ate_Peticiones_Datos.Consulta_Parametros().Rows[0][Presidencia.Constantes.Apl_Parametros.Campo_Password_Correo].ToString();
                mail.P_Subject = "SEGUIMIENTO A SOLICITUD DE TRAMITE: " + Txt_Nombre_Tramite.Text;

                //  mensaje
                String Mensaje = "Al Cuidadano " + Txt_Solicito.Text + "<br /><br />Se le notifica que la Solicitud para el Tramite \"" + Txt_Nombre_Tramite.Text + "\"";
                if (Solicitud.P_Estatus.Equals("TERMINADO"))
                {
                    Mensaje = Mensaje + " ha <b>FINALIZADO</b> de manera exitosa." + "<br /><br /><b>NOTA:</b><br />" + Solicitud.P_Comentarios + ".";
                }
                else if (Solicitud.P_Estatus.Equals("DETENIDO"))
                {
                    Mensaje = Mensaje + " ha sido <b>DETENIDA</b>." + "<br /><br /><b>CAUSA:</b><br />" + Solicitud.P_Comentarios + ".";
                }
                else if (Solicitud.P_Estatus.Equals("CANCELADO"))
                {
                    Mensaje = Mensaje + " ha sido <b>CANCELADA</b>." + "<br /><br /><b>CAUSA:</b><br />" + Solicitud.P_Comentarios + ".";
                }
                Mensaje = Mensaje + "<br /><br />Por su Atenci&oacute;n </b>Gracias<br />";
                Mensaje = Mensaje + "<hr width=\"98%\"><br />PRESIDENCIA MUNICIPAL DE IRAPUATO, GTO";


                mail.P_Texto = HttpUtility.HtmlDecode(Mensaje);
                mail.Enviar_Correo();
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error(true, Ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Ocultar_Div
    ///DESCRIPCIÓN: 
    ///PARAMETROS:     
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  23/Agosto/2012 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///******************************************************************************* 
    private void Ocultar_Div(Boolean Estado)
    {
         try
         {
             if (Estado == true)
             {
                 Div_Detalle_Bandeja_Tramite.Style.Value = "display:block";
                 Div_Tramite_id.Style.Value = "display:none";
                 Btn_Cancelar_Solicitud.Visible = true;
             }
             else
             {
                 Div_Detalle_Bandeja_Tramite.Style.Value = "display:none";
                 Div_Tramite_id.Style.Value = "overflow: auto; height: 400px; width: 98%; vertical-align: top; border-style: hidden;margin-top:5px; border-color: Silver; display: block";
                 Btn_Cancelar_Solicitud.Visible = false;
                 Configuracion_Catalogo(false);

             }
            
         }
         catch (Exception Ex)
         {
             Mensaje_Error(true, Ex.Message.ToString());
         }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Enviar_Correo_Notificacion
    ///DESCRIPCIÓN: Envia un correo al Usuario cuando el estatus de la solicitud cambio
    ///             a 'DETENIDO', 'CANDELADO' ó 'TERMINADO'.
    ///PARAMETROS:     
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  14/Mayo/2012 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///******************************************************************************* 
    private void Enviar_Correo_Notificacion(Cls_Ope_Bandeja_Tramites_Negocio Solicitud, String Estatus_Anterior , String Actividad)
    {
        MailMessage Correo = new MailMessage(); 
        String Ruta_Archivo_Adjunto = "";
        String Nombre_Reporte_Orden_Pago = "";
        String Para = "";
        String De = "";
        String Puerto = "";
        String Servidor = "";
        String Contraseña = "";
        String Mensaje = ""; 
        Boolean Enviar_Correo_Orden_Pago = false;
        Boolean Mensaje_Repetido = false;
        var Obj_Parametros = new Cls_Cat_Tra_Parametros_Negocio();
        try
        {

            if (Solicitud.P_Tipo_Actividad == "COBRO")
            {
                Enviar_Correo_Orden_Pago = true;
                Nombre_Reporte_Orden_Pago = Imprimir_Reporte("PDF", Solicitud, "INTERNO");
            }
            // consultar parámetros
            Obj_Parametros.Consultar_Parametros();


            Para = Solicitud.P_Email;
            De = Cls_Cat_Ate_Peticiones_Datos.Consulta_Parametros().Rows[0][Presidencia.Constantes.Apl_Parametros.Campo_Correo_Saliente].ToString();
            Contraseña = Cls_Cat_Ate_Peticiones_Datos.Consulta_Parametros().Rows[0][Presidencia.Constantes.Apl_Parametros.Campo_Password_Correo].ToString();
            Servidor = Cls_Cat_Ate_Peticiones_Datos.Consulta_Parametros().Rows[0][Presidencia.Constantes.Apl_Parametros.Campo_Servidor_Correo].ToString();
            Puerto = "25";

            Correo.To.Add(Para);
            Correo.From = new MailAddress(De, Obj_Parametros.P_Correo_Encabezado);//    va el asunto que se obtiene de los parametros
            Correo.Subject = "Tramite - " + Txt_Nombre_Tramite.Text.ToUpper().Trim();
            Correo.SubjectEncoding = System.Text.Encoding.UTF8;

            if (Enviar_Correo_Orden_Pago == true)
            {
                Ruta_Archivo_Adjunto = MapPath("../../Reporte/" + Nombre_Reporte_Orden_Pago);

                Attachment Datos_Adjuntos = new Attachment(Ruta_Archivo_Adjunto, MediaTypeNames.Application.Pdf);
                //Obtengo las propiedades del archivo.
                ContentDisposition Propiedades_Archivo = Datos_Adjuntos.ContentDisposition;
                Propiedades_Archivo.CreationDate = System.IO.File.GetCreationTime(Ruta_Archivo_Adjunto);
                Propiedades_Archivo.ModificationDate = System.IO.File.GetLastWriteTime(Ruta_Archivo_Adjunto);
                Propiedades_Archivo.ReadDate = System.IO.File.GetLastAccessTime(Ruta_Archivo_Adjunto);
                //Agrego el archivo al mensaje
                Correo.Attachments.Add(Datos_Adjuntos);
            }


            /**********************estructura del documento:*******************************
             * secdcion del titulo del correo (ejemplo: seguimiento de solictud)
             * fecha
             * cuerpo del correo
             * informacion de la solicutd y estatus
             * despedida
             * persona quien firma
             * 
             *******************************************************************************/

            //  para el texto del mensaje
            Mensaje = "<html>";
            Mensaje += "<body> ";

            //  seccion del titulo del correo
            Mensaje += "<table width=\"100%\" > ";
            Mensaje += "<tr>";
            Mensaje += "<td style=\"width:100%;font-size:18px;\" align=\"center\" >";
            Mensaje += Obj_Parametros.P_Correo_Encabezado.ToUpper().Trim();
            Mensaje += "</td>";
            Mensaje += "</hr>";
            Mensaje += "</table>";

            Mensaje += "<br>";

            //  seccion de la fecha del documento
            Mensaje += "<table width=\"100%\" > ";
            Mensaje += "<tr>";
            Mensaje += "<td style=\"width:100%;font-size:14px;\" align=\"right\" >";
            Mensaje += "Irapuato, Guanajuato a " + String.Format("{0:D}", DateTime.Now) + ".";
            Mensaje += "</td>";
            Mensaje += "</hr>";
            Mensaje += "</table>";


            //  seccion del cuerpo del correo
            Mensaje += "<table width=\"100%\" > ";
            Mensaje += "<tr>";
            Mensaje += "<td style=\"width:100%;font-size:14px;\" align=\"left\" >";
            Mensaje += Obj_Parametros.P_Correo_Cuerpo;
            Mensaje += "</td>";
            Mensaje += "</hr>";
            Mensaje += "</table>";

            //  linea
            Mensaje += "<hr width=\"98%\">";

            //  informacion general de la solicitud
            Mensaje += "<br> <p align=justify style=\"font-size:14px\"> ";
            Mensaje += "Información general de la solicitud:";
            Mensaje += "<br> Nombre del solicitante <b>" + Txt_Solicito.Text + "</b> ";
            Mensaje += "<br> Solicitud de Tramite <b>" + Txt_Nombre_Tramite.Text + "</b> ";
            Mensaje += "<br> Con Folio <b>" + Txt_Clave_Solicitud.Text.Trim() + "<b>. <br>";

            //  estatus terminado
            if (Solicitud.P_Estatus.Equals("TERMINADO"))
            {
                Mensaje += "<br> Ha  <b>FINALIZADO</b> de manera exitosa.";
                //if (!String.IsNullOrEmpty(Solicitud.P_Comentarios))
                //    Mensaje += "<br><br> <b>NOTA:</b> " + Solicitud.P_Comentarios + ".";
            }

            //  estatus detenido
            else if (Solicitud.P_Estatus.Equals("DETENIDO"))
            {
                Mensaje += "<br>  Ha sido <b>DETENIDA</b>.";
                Mensaje += "<br><b>POR LA CAUSA:</b> " + Solicitud.P_Comentarios;
                Mensaje += "."; //  punto final de la causa
                Mensaje_Repetido = true;
            }

            //  estatus cancelado
            else if (Solicitud.P_Estatus.Equals("CANCELADO"))
            {
                Mensaje += "<br>  Ha sido <b>CANCELADA</b>.";
                Mensaje += "<br><b>POR LA CAUSA:</b> " + Solicitud.P_Comentarios;
                Mensaje += "."; //  punto final de la causa
                Mensaje_Repetido = true;
            }

            //  VALIDACION PARA PODER IMPRIMIR EL REPORTE DE LA SOLICITUD
            if (Estatus_Anterior == "PENDIENTE" && Cmb_Evaluacion.SelectedValue == "APROBAR")
            {
                Mensaje += "<br><b>SU TRAMITE HA SIDO RECIBIDO Y CANALIZADO AL AREA CORRESPONDIENTE PARA SER ATENDIDO, USTED YA PUEDE IMPRIMIR EL FORMATO DE SOLICITUD DE TRAMITE</b> ";
                Mensaje += "."; //  punto final de la causa
                Mensaje_Repetido = true;
            }

            //  validacion para cuando se cumple con una actividad
            else if (Estatus_Anterior == "PROCESO")
            {
                if (Mensaje_Repetido != true)
                {
                    Mensaje += "<br>  Ha pasado <b> la actividad  </b>.";
                    Mensaje += "<br><b>" + Actividad + "</b> ";
                    Mensaje += "."; //  punto final de la causa
                }
            }

            Mensaje += "</p>";
            Mensaje += "<hr width=\"98%\">";

            //  Seccion de la despedida
            Mensaje += "<br>" + "<p align =left> <b> " + Obj_Parametros.P_Correo_Despedida + " </b>.</p> ";

            //  Seccion de quien firma el correo
            Mensaje += "<br> <p align =center> Atentemente </b> </p> ";
            Mensaje += "<br> <p align =center>" + Obj_Parametros.P_Correo_Firma + "</b> </p> ";

            Mensaje += "</body>";
            Mensaje += "</html>";

            Mensaje = HttpUtility.HtmlDecode(Mensaje);

            if ((!Correo.From.Equals("") || Correo.From != null) && (!Correo.To.Equals("") || Correo.To != null))
            {
                Correo.Body = Mensaje;

                Correo.BodyEncoding = System.Text.Encoding.UTF8;
                Correo.IsBodyHtml = true;

                SmtpClient cliente_correo = new SmtpClient();
                cliente_correo.Port = int.Parse(Puerto);
                cliente_correo.UseDefaultCredentials = true;
                //cliente_correo.Credentials = new System.Net.NetworkCredential(De, Contraseña);
                cliente_correo.Credentials = new System.Net.NetworkCredential(De, Contraseña);
                cliente_correo.Host = Servidor;
                cliente_correo.Send(Correo);
                Correo = null;
            }
            else
            {
                throw new Exception("No se tiene configurada una cuenta de correo, favor de notificar");
            }
        }// fin del try
        catch (Exception Ex)
        {
            Mensaje_Error(true, Ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Imprimir
    ///DESCRIPCIÓN          : Manda a imprimir el reporte con el formato indicado
    ///PARAMETROS:     
    ///CREO                 : Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO           : 17/Septiembre/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected String Imprimir_Reporte(String Formato, Cls_Ope_Bandeja_Tramites_Negocio Solicitud, String Tipo_Reporte)
    {
        String Nombre_Repote_Crystal = "Rpt_Ing_Orden_Pago.rpt";
        String Nombre_Reporte = "Reporte de Orden de Pago_";

        Cls_Ope_Bandeja_Tramites_Negocio Negocio_Solicitud = new Cls_Ope_Bandeja_Tramites_Negocio();
        Cls_Cat_Tramites_Negocio Negocio_Tramite = new Cls_Cat_Tramites_Negocio();
        Cls_Rpt_Ven_Consultar_Tramites_Negocio Negocio_Consulta_Subconcepto = new Cls_Rpt_Ven_Consultar_Tramites_Negocio();

        Ds_Ope_Ing_Orden_Pago Ds_Orden_Pago = new Ds_Ope_Ing_Orden_Pago();
        DataTable Dt_Orden_Pago = Ds_Orden_Pago.Tables["Dt_Orden_Pago"].Copy();
        DataTable Dt_Conceptos_Orden_Pago = Ds_Orden_Pago.Tables["Dt_Conceptos_Orden_Pago"].Copy();
        DataTable Dt_Contribuyente = Ds_Orden_Pago.Tables["Dt_Contribuyente"].Copy();
        DataTable Dt_Descuentos_Orden_Pago = Ds_Orden_Pago.Tables["Dt_Descuentos_Orden_Pago"].Copy();
        DataTable Dt_Datos_Ciudadano = Cls_Sessiones.Datos_Ciudadano;
        DataTable Dt_Consulta = new DataTable();
        DataTable Dt_Consulta_Fecha_Pasivo = new DataTable();
        DataTable Dt_Consulta_Contribuyente = new DataTable();

        // Se obtienen los Datos a Detalle de la Solicitud Seleccionada
        Negocio_Solicitud.P_Solicitud_ID = Hdf_Solicitud_ID.Value;
        Negocio_Solicitud = Negocio_Solicitud.Consultar_Datos_Solicitud();

        // Se obtienen los Datos del tramite
        Negocio_Tramite.P_Tramite_ID = Negocio_Solicitud.P_Tramite_id;
        Negocio_Tramite = Negocio_Tramite.Consultar_Datos_Tramite();

        //  se obtiene el nombre de la clave y fundamento
        Negocio_Consulta_Subconcepto.P_Clave = Negocio_Tramite.P_Cuenta_ID;
        Dt_Consulta = Negocio_Consulta_Subconcepto.Consultar_Clave_Fundamento();

        //  se consulta la fecha del pasivo
        Negocio_Consulta_Subconcepto.P_Clave_Solicitud = Negocio_Solicitud.P_Clave_Solicitud;
        Dt_Consulta_Fecha_Pasivo = Negocio_Consulta_Subconcepto.Consultar_Fecha_Pasivo();

        //  para la orden de pago
        DataRow Dr_Orden_Pago;
        Dr_Orden_Pago = Dt_Orden_Pago.NewRow();
        Dr_Orden_Pago["NO_ORDEN_PAGO"] = "00000";
        Dr_Orden_Pago["AÑO"] = DateTime.Today.Year;
        Dr_Orden_Pago["NOMBRE_CONTRIBUYENTE"] = Solicitud.P_Solicito;
        Dr_Orden_Pago["CONTRIBUYENTE_ID"] = Hdf_Contribuyente_Id.Value;
        Dr_Orden_Pago["FOLIO"] = Negocio_Solicitud.P_Clave_Solicitud.Trim();
        if (Dt_Consulta_Fecha_Pasivo is DataTable)
        {
            if (Dt_Consulta_Fecha_Pasivo != null && Dt_Consulta_Fecha_Pasivo.Rows.Count > 0)
            {
                foreach (DataRow Registro in Dt_Consulta_Fecha_Pasivo.Rows)
                {
                    if (Registro is DataRow)
                    {
                        Dr_Orden_Pago["FECHA_CREO"] = Registro[Ope_Ing_Pasivo.Campo_Fecha_Creo].ToString();
                    }
                }
            }
        }
        Dt_Orden_Pago.Rows.Add(Dr_Orden_Pago);
        Dt_Orden_Pago.TableName = "Dt_Orden_Pago";

        //  para el concepto de orden de pago
        DataRow Dr_Concepto_Orden_Pago;
        Dr_Concepto_Orden_Pago = Dt_Conceptos_Orden_Pago.NewRow();
        Dr_Concepto_Orden_Pago["NO_ORDEN_PAGO"] = "00000";
        Dr_Concepto_Orden_Pago["AÑO"] = DateTime.Today.Year;
        if (Dt_Consulta is DataTable)
        {
            if (Dt_Consulta != null && Dt_Consulta.Rows.Count > 0)
            {
                foreach (DataRow Registro in Dt_Consulta.Rows)
                {
                    if (Registro is DataRow)
                    {
                        Dr_Concepto_Orden_Pago["CLAVE_INGRESO"] = Registro[Cat_Psp_SubConcepto_Ing.Campo_Descripcion].ToString();
                        Dr_Concepto_Orden_Pago["FUNDAMENTO"] = Registro[Cat_Psp_SubConcepto_Ing.Campo_Fundamento].ToString();
                    }
                }
            }
        }
        if (Dt_Consulta_Fecha_Pasivo is DataTable)
        {
            if (Dt_Consulta_Fecha_Pasivo != null && Dt_Consulta_Fecha_Pasivo.Rows.Count > 0)
            {
                foreach (DataRow Registro in Dt_Consulta_Fecha_Pasivo.Rows)
                {
                    if (Registro is DataRow)
                    {
                        Dr_Concepto_Orden_Pago["OBSERVACIONES"] = Registro[Ope_Ing_Pasivo.Campo_Origen].ToString();
                    }
                }
            }
        }
        Dr_Concepto_Orden_Pago["HONORARIOS"] = 0;
        Dr_Concepto_Orden_Pago["MULTAS"] = 0;
        Dr_Concepto_Orden_Pago["DESCUENTO_IMPORTE"] = 0;
        Dr_Concepto_Orden_Pago["AJUSTE_TARIFARIO"] = 0;
        Dr_Concepto_Orden_Pago["MONTO_IMPORTE"] = 0;
        Dr_Concepto_Orden_Pago["MORATORIOS"] = 0;
        Dr_Concepto_Orden_Pago["RECARGOS"] = 0;

        //if (Tipo_Reporte == "INTERNO")
        Dr_Concepto_Orden_Pago["IMPORTE"] = Convert.ToDouble(Hdf_Costo_Total.Value);
        //else
        //    Dr_Concepto_Orden_Pago["IMPORTE"] = Convert.ToDouble(Solicitud.P_Costo_Base);

        Dr_Concepto_Orden_Pago["REFERENCIA"] = Solicitud.P_Clave_Solicitud;
        Dr_Concepto_Orden_Pago["UNIDADES"] = Convert.ToDouble(Solicitud.P_Unidades);

        //if (Tipo_Reporte == "INTERNO")
        Dr_Concepto_Orden_Pago["TOTAL"] = Convert.ToDouble(Hdf_Costo_Total.Value);
        //else
        //    Dr_Concepto_Orden_Pago["TOTAL"] = Convert.ToDouble(Solicitud.P_Costo_Total);

        Dt_Conceptos_Orden_Pago.Rows.Add(Dr_Concepto_Orden_Pago);
        Dt_Conceptos_Orden_Pago.TableName = "Dt_Conceptos_Orden_Pago";

        //  para la informacion del contribuyente
        DataRow Dr_Contribuyente;
        Dt_Consulta_Contribuyente = Solicitud.Consultar_Datos_Contribuyente();
        if (Dt_Consulta_Contribuyente is DataTable)
        {
            if (Dt_Consulta_Contribuyente != null && Dt_Consulta_Contribuyente.Rows.Count > 0)
            {
                foreach (DataRow Registro in Dt_Consulta_Contribuyente.Rows)
                {
                    if (Registro is DataRow)
                    {
                        Dr_Contribuyente = Dt_Contribuyente.NewRow();
                        Dr_Contribuyente["CONTRIBUYENTE_ID"] = Hdf_Contribuyente_Id.Value;
                        Dr_Contribuyente["RFC"] = Registro[Cat_Pre_Contribuyentes.Campo_RFC].ToString();
                        Dr_Contribuyente["CURP"] = Registro[Cat_Pre_Contribuyentes.Campo_CURP].ToString();
                        Dr_Contribuyente["IFE"] = Registro[Cat_Pre_Contribuyentes.Campo_CURP].ToString();
                        Dr_Contribuyente["SEXO"] = Registro[Cat_Pre_Contribuyentes.Campo_Sexo].ToString();
                        Dr_Contribuyente["ESTATUS"] = Registro[Cat_Pre_Contribuyentes.Campo_Estatus].ToString();
                        Dr_Contribuyente["CALLE_FISCAL"] = Registro[Cat_Pre_Contribuyentes.Campo_Calle_Ubicacion].ToString();
                        Dr_Contribuyente["COLONIA_FISCAL"] = Registro[Cat_Pre_Contribuyentes.Campo_Colonia_Ubicacion].ToString();
                        Dr_Contribuyente["NO_EXTERIOR_FISCAL"] = "";
                        Dr_Contribuyente["NO_INTERIOR_FISCAL"] = "";
                        Dr_Contribuyente["ESTADO_FISCAL"] = Registro[Cat_Pre_Contribuyentes.Campo_Estado_Ubicacion].ToString();
                        Dr_Contribuyente["CIUDAD_FISCAL"] = Registro[Cat_Pre_Contribuyentes.Campo_Ciudad_Ubicacion].ToString();
                        Dt_Contribuyente.Rows.Add(Dr_Contribuyente);
                    }
                }
            }
        }
        Dt_Contribuyente.TableName = "Dt_Contribuyente";

        DataRow Dr_Descuentos_Orden_Pago;

        Dr_Descuentos_Orden_Pago = Dt_Descuentos_Orden_Pago.NewRow();
        Dr_Descuentos_Orden_Pago["NOMBRE_CONTRIBUYENTE"] = Cls_Sessiones.Nombre_Ciudadano;
        Dr_Descuentos_Orden_Pago[Ope_Ing_Descuentos.Campo_Referencia] = Dt_Orden_Pago.Rows[0][Ope_Ing_Ordenes_Pago.Campo_Folio].ToString();// Txt_Referencia.Text.Trim();
        Dr_Descuentos_Orden_Pago["HONORARIOS"] = 0;//Convert.ToDecimal(Txt_Honorarios.Text);
        Dr_Descuentos_Orden_Pago["MULTAS"] = 0;//Convert.ToDecimal(Txt_Multas.Text);
        Dr_Descuentos_Orden_Pago["MORATORIOS"] = 0;//Convert.ToDecimal(Txt_Moratorios.Text);
        Dr_Descuentos_Orden_Pago["RECARGOS"] = 0;//Convert.ToDecimal(Txt_Recargos.Text);
        Dr_Descuentos_Orden_Pago[Ope_Ing_Descuentos.Campo_Monto_Honorarios] = 0;// Convert.ToDecimal(Txt_Monto_Honorarios.Text);
        Dr_Descuentos_Orden_Pago[Ope_Ing_Descuentos.Campo_Monto_Multas] = 0;
        Dr_Descuentos_Orden_Pago[Ope_Ing_Descuentos.Campo_Monto_Moratorios] = 0;
        Dr_Descuentos_Orden_Pago[Ope_Ing_Descuentos.Campo_Monto_Recargos] = 0;
        Dr_Descuentos_Orden_Pago[Ope_Ing_Descuentos.Campo_Descuento_Honorarios] = 0;// Convert.ToDecimal(Txt_Descuento_Honorarios.Text);
        Dr_Descuentos_Orden_Pago[Ope_Ing_Descuentos.Campo_Descuento_Multas] = 0;
        Dr_Descuentos_Orden_Pago[Ope_Ing_Descuentos.Campo_Descuento_Moratorios] = 0;
        Dr_Descuentos_Orden_Pago[Ope_Ing_Descuentos.Campo_Descuento_Recargos] = 0;
        Dr_Descuentos_Orden_Pago[Ope_Ing_Descuentos.Campo_Descuento_Recargos] = 0;

        //if (Tipo_Reporte == "INTERNO")
        Dr_Descuentos_Orden_Pago["TOTAL"] = Convert.ToDouble(Hdf_Costo_Total.Value);
        //else
        //    Dr_Descuentos_Orden_Pago["TOTAL"] = Convert.ToDouble(Solicitud.P_Costo_Total);

        Dr_Descuentos_Orden_Pago["REALIZO"] = Cls_Sessiones.Nombre_Empleado;
        Dt_Descuentos_Orden_Pago.Rows.Add(Dr_Descuentos_Orden_Pago);
        Dt_Descuentos_Orden_Pago.TableName = "Dt_Descuentos_Orden_Pago";

        Ds_Orden_Pago.Clear();
        Ds_Orden_Pago.Tables.Clear();
        Ds_Orden_Pago.Tables.Add(Dt_Orden_Pago.Copy());
        Ds_Orden_Pago.Tables.Add(Dt_Conceptos_Orden_Pago.Copy());
        Ds_Orden_Pago.Tables.Add(Dt_Contribuyente.Copy());
        Ds_Orden_Pago.Tables.Add(Dt_Descuentos_Orden_Pago.Copy());
        String Nombre_Reporte_Generado = Generar_Reportes(Ds_Orden_Pago, Nombre_Repote_Crystal, Nombre_Reporte, Formato, Tipo_Reporte);

        return Nombre_Reporte_Generado;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Generar_Reportes
    ///DESCRIPCIÓN          : Prepara la información necesaria para generar el reporte
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 12/Junio/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected String Generar_Reportes(DataSet Ds_Rpt_Orden_Pago, String Nombre_Reporte_Crystal, String Nombre_Reporte, String Formato, String Uso_Reporte)
    {
        String Ruta_Reporte_Crystal = "";
        String Nombre_Reporte_Generar = "";

        // Ruta donde se encuentra el reporte Crystal
        Ruta_Reporte_Crystal = "../Rpt/Ingresos/" + Nombre_Reporte_Crystal;

        // Se crea el nombre del reporte
        String Nombre_Report = Nombre_Reporte + "_" + Session.SessionID;

        // Se da el nombre del reporte que se va generar
        if (Formato == "PDF")
            Nombre_Reporte_Generar = Nombre_Report + ".pdf";  // Es el nombre del reporte PDF que se va a generar
        else if (Formato == "Excel")
            Nombre_Reporte_Generar = Nombre_Report + ".xls";  // Es el nombre del repote en Excel que se va a generar

        Cls_Reportes Reportes = new Cls_Reportes();
        Reportes.Generar_Reporte(ref Ds_Rpt_Orden_Pago, Ruta_Reporte_Crystal, Nombre_Reporte_Generar, Formato);

        if (Uso_Reporte == "INTERNO")
        {
            Abrir_Ventana(Nombre_Reporte_Generar);
        }

        return Nombre_Reporte_Generar;
    }
    
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Abrir_Ventana
    ///DESCRIPCIÓN: Abre en otra ventana el archivo pdf
    ///PARÁMETROS : Nombre_Archivo: Guarda el nombre del archivo que se desea abrir
    ///                             para mostrar los datos al usuario
    ///CREO       : Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO  : 30-Junio-2012
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************
    private void Abrir_Ventana(String Nombre_Archivo)
    {
        String Ruta = "";
        try
        {
            Ruta = "../../Reporte/" + Nombre_Archivo;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + Ruta + "', 'Reportes','toolbar=0,dire ctories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
        }
        catch (Exception ex)
        {
            throw new Exception("Abrir_Ventana " + ex.Message.ToString(), ex);
        }
    }
    
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Cargar_Grid_Comentarios
    /// DESCRIPCION :cargara los comentarios
    /// PARAMETROS  : 
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 24/Julio/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Cargar_Grid_Comentarios(DataTable Dt_Consulta)
    {
        try
        {
            if (Dt_Consulta != null && Dt_Consulta.Rows.Count > 0)
            {
                Grid_Comentarios_Internos.DataSource = Dt_Consulta;
                Grid_Comentarios_Internos.DataBind();
            }
            else
            {
                Grid_Comentarios_Internos.DataSource = new DataTable();
                Grid_Comentarios_Internos.DataBind();
            }

        }
        catch (Exception ex)
        {
            throw new Exception("Cargar_Grid " + ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Cargar_Grid_Actividades
    /// DESCRIPCION :cargara las actividades
    /// PARAMETROS  : 
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 20/Junio/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private String[,] Obtener_Datos_Dictamen()
    {
        DataTable Dt_Datos = (DataTable)(Session["Grid_Datos"]);
        String[,] Datos = new String[Dt_Datos.Rows.Count, 2];
        try
        {
            for (int Contador_For = 0; Contador_For < Dt_Datos.Rows.Count; Contador_For++)
            {

                Datos[Contador_For, 0] = Dt_Datos.Rows[Contador_For].ItemArray[0].ToString();

                String Temporal = Grid_Datos_Dictamen.Rows[Contador_For].Cells[0].Text;

                String Valor_Dato = ((TextBox)Grid_Datos_Dictamen.Rows[Contador_For].FindControl("Txt_Descripcion_Datos")).Text;

                if (Valor_Dato != "" || (Dt_Datos.Rows[Contador_For][Cat_Tra_Datos_Tramite.Campo_Dato_Requerido].ToString()) == "N")
                    Datos[Contador_For, 1] = Valor_Dato;
            }

            return Datos;
        }
        catch (Exception ex)
        {
            throw new Exception("Cargar_Grid " + ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Obtener_Datos_Finales_Replica_Solicitud
    /// DESCRIPCION :cargara los datos finales de la solicitud que se replica
    /// PARAMETROS  : 
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 07/Noviembre/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private String[,] Obtener_Datos_Finales_Replica_Solicitud(DataTable Dt_Datos_Finales)
    {
        DataTable Dt_Datos = Dt_Datos_Finales;
        String[,] Datos = new String[Dt_Datos.Rows.Count, 2];
        try
        {
            for (int Contador_For = 0; Contador_For < Dt_Datos.Rows.Count; Contador_For++)
            {
                Datos[Contador_For, 0] = Dt_Datos.Rows[Contador_For].ItemArray[0].ToString();
                Datos[Contador_For, 1] = "";
                //String Temporal = Grid_Datos_Dictamen.Rows[Contador_For].Cells[0].Text;

                //String Valor_Dato = ((TextBox)Grid_Datos_Dictamen.Rows[Contador_For].FindControl("Txt_Descripcion_Datos")).Text;

                //if (Valor_Dato != "" || (Dt_Datos.Rows[Contador_For][Cat_Tra_Datos_Tramite.Campo_Dato_Requerido].ToString()) == "N")
                //    Datos[Contador_For, 1] = Valor_Dato;a
            }

            return Datos;
        }
        catch (Exception ex)
        {
            throw new Exception("Cargar_Grid " + ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cargar_Grid_Datos_Dictamen
    ///DESCRIPCIÓN: Carga el grid de datos 
    ///PARAMETROS:      String Tramite_ID:contiene el id del tramite
    ///CREO:            HUGO ENRIQUE RAMÍREZ AGUILERA
    ///FECHA_CREO:      20/Agosto/2012 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Cargar_Grid_Datos_Dictamen(String Tramite_ID)
    {
        Cls_Ope_Solicitud_Tramites_Negocio Negocio_Consultar_Datos_Dictamen = new Cls_Ope_Solicitud_Tramites_Negocio(); ;
        try
        {
            Negocio_Consultar_Datos_Dictamen.P_Tramite_ID = Tramite_ID;
            Negocio_Consultar_Datos_Dictamen.P_Tipo_Dato = "FINAL";
            DataSet Ds_Datos = new DataSet();
            Ds_Datos = Negocio_Consultar_Datos_Dictamen.Consultar_Datos_Tramite();
            
            //  se ordenara la tabla por fecha
            DataView Dv_Ordenar = new DataView(Ds_Datos.Tables[0]);
            Dv_Ordenar.Sort = Cat_Tra_Datos_Tramite.Campo_Dato_ID;//SOLICITO asc, 
            DataTable Dt_Datos_Ordenados = Dv_Ordenar.ToTable();
            Session["Grid_Datos"] = Dt_Datos_Ordenados;


            if (Dt_Datos_Ordenados.Rows.Count > 0)
                Grid_Datos_Dictamen.DataSource = Dt_Datos_Ordenados;
           
            else
                Grid_Datos_Dictamen.DataSource = new DataTable();

            Grid_Datos_Dictamen.DataBind();

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cargar_Grid_Datos_Dictamen_Modificar
    ///DESCRIPCIÓN: Carga el grid de datos 
    ///PARAMETROS:      String Tramite_ID:contiene el id del tramite
    ///CREO:            HUGO ENRIQUE RAMÍREZ AGUILERA
    ///FECHA_CREO:      20/Agosto/2012 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Cargar_Grid_Datos_Dictamen_Modificar(DataTable Dt_Consulta)
    {
        try
        {
            Grid_Datos_Dictamen_Modificar.Columns[0].Visible = true;
            Grid_Datos_Dictamen_Modificar.Columns[1].Visible = true;

            //  se ordenara la tabla por fecha
            DataView Dv_Ordenar = new DataView(Dt_Consulta);
            Dv_Ordenar.Sort = Cat_Tra_Datos_Tramite.Campo_Dato_ID;//SOLICITO asc, 
            DataTable Dt_Datos_Ordenados = Dv_Ordenar.ToTable();

            if (Dt_Datos_Ordenados.Rows.Count > 0)
            {
                Grid_Datos_Dictamen_Modificar.DataSource = Dt_Datos_Ordenados;
                Session["Grid_Datos_Modificar"] = Dt_Datos_Ordenados;
            }

            else
                Grid_Datos_Dictamen_Modificar.DataSource = new DataTable();
            
            Grid_Datos_Dictamen_Modificar.DataBind();
            Grid_Datos_Dictamen_Modificar.Columns[0].Visible = false;
            Grid_Datos_Dictamen_Modificar.Columns[1].Visible = false;

            //Se cargará el valor del Dato
            if (Grid_Datos_Dictamen_Modificar.Rows.Count == Dt_Datos_Ordenados.Rows.Count)
            {
                for (Int32 Contador = 0; Contador < Grid_Datos_Dictamen_Modificar.Rows.Count; Contador++)
                {
                    TextBox Txt_Valor_Temporal = (TextBox)Grid_Datos_Dictamen_Modificar.Rows[Contador].Cells[3].Controls[1];
                    Txt_Valor_Temporal.Text = Dt_Datos_Ordenados.Rows[Contador][3].ToString();
                }
            }

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }
    
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Cargar_Grid_Actividades
    /// DESCRIPCION :cargara las actividades
    /// PARAMETROS  : 
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 20/Junio/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Cargar_Grid_Actividades(DataTable Dt_Consulta)
    {

        try
        {
            if (Dt_Consulta != null && Dt_Consulta.Rows.Count > 0)
            {
                Grid_Actividades.DataSource = Dt_Consulta;
                Grid_Actividades.DataBind();
            }
            else
            {
                Grid_Actividades.DataSource = new DataTable();
                Grid_Actividades.DataBind();
            }

        }
        catch (Exception ex)
        {
            throw new Exception("Cargar_Grid " + ex.Message.ToString());
        }
    }
   
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Cargar_Ventana_Emergente_Resumen_Predio
    ///DESCRIPCIÓN          : Establece el evento onclik del control para abrir la ventana emergente del Resumen de Predial con la ruta y parámtros necesarios
    ///PARAMETROS: 
    ///CREO                 : Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO           : 09/Julio/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Cargar_Ventana_Emergente_Resumen_Predio()
    {
        String Ventana_Modal = "Abrir_Resumen('../Ventanilla/Ventanas_Emergentes/Frm_Resumen_Predio_Ventanilla.aspx";
        String Propiedades = ", 'center:yes,resizable=no,status=no,width=750,scrollbars=yes,');";
        Btn_Buscar_Cuenta_Predial.Attributes.Add("onclick", Ventana_Modal + "?Cuenta_Predial=" + Txt_Cuenta_Predial.Text.Trim() + "'" + Propiedades);
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Llenar_Combos
    ///DESCRIPCIÓN: Consulta las zonas y empleados (supervisores) de ordenamiento territorial y los carga en el combo
    ///PARÁMETROS:
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 16-jul-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Llenar_Combos(String Area_Dependencia)
    {
        Cls_Cat_Ort_Zona_Negocio Neg_Consulta_Zonas = new Cls_Cat_Ort_Zona_Negocio(); 
        Cls_Ope_Ort_Bitacora_Documentos_Negocio Negocio_Consultar_Solicitud = new Cls_Ope_Ort_Bitacora_Documentos_Negocio();
        Cls_Cat_Tramites_Negocio Negocio_Consultar_Tramites = new Cls_Cat_Tramites_Negocio();
        DataTable Dt_Zonas;
        DataTable Dt_Zona_ID;
        DataTable Dt_Supervisores;
        DataTable Dt_Tramites;

        Cls_Cat_Ort_Parametros_Negocio Obj_Parametros = new Cls_Cat_Ort_Parametros_Negocio();
        string Dependencia_ID_Ordenamiento = "";
        string Dependencia_ID_Ambiental = "";
        string Dependencia_ID_Urbanistico = "";
        string Dependencia_ID_Inmobiliario = "";
        DataTable Dt_Consulta_Tramites = new DataTable();
        StringBuilder Expresion_Sql = new StringBuilder();

        try
        {
            // consultar zonas
            Neg_Consulta_Zonas.P_Nombre = Area_Dependencia.Trim();
            Dt_Zona_ID = Neg_Consulta_Zonas.Consultar_Area_Id();

            Neg_Consulta_Zonas.P_Nombre = String.Empty;
            Neg_Consulta_Zonas.P_Area_ID = Dt_Zona_ID.Rows[0][Cat_Areas.Campo_Area_ID].ToString();
            Dt_Zonas = Neg_Consulta_Zonas.Consultar_Zonas();
            Dt_Zonas.DefaultView.Sort = Cat_Ort_Zona.Campo_Nombre + " ASC";
            // cargar en el combo zonas
            Cmb_Zonas.DataSource = Dt_Zonas;
            Cmb_Zonas.DataValueField = Cat_Ort_Zona.Campo_Zona_ID;
            Cmb_Zonas.DataTextField = Cat_Ort_Zona.Campo_Nombre;
            Cmb_Zonas.DataBind();
            Cmb_Zonas.Items.Insert(0, "< SELECCIONE >");
            
            // consultar supervisores
            Dt_Supervisores = Negocio_Consultar_Solicitud.Consultar_Personal();
            Dt_Supervisores.DefaultView.Sort = "Nombre_Usuario ASC";
            
            // cargar en el combo supervisores
            Cmb_Supervisor_Zona.DataSource = Dt_Supervisores;
            Cmb_Supervisor_Zona.DataValueField = Cat_Empleados.Campo_Empleado_ID;
            Cmb_Supervisor_Zona.DataTextField = "Nombre_Usuario";
            Cmb_Supervisor_Zona.DataBind();
            Cmb_Supervisor_Zona.Items.Insert(0, "< SELECCIONE >");

            var Obj_Peritos = new Cls_Cat_Ort_Inspectores_Negocio();
            DataTable Dt_Peritos;
            
            // consultar peritos
            Dt_Peritos = Obj_Peritos.Consultar_Inspectores();
            
            // cargar datos en el combo
            Cmb_Perito.Items.Clear();
            Cmb_Perito.DataSource = Dt_Peritos;
            Cmb_Perito.DataTextField = Cat_Ort_Inspectores.Campo_Nombre;
            Cmb_Perito.DataValueField = Cat_Ort_Inspectores.Campo_Inspector_ID;
            Cmb_Perito.DataBind();
            Cmb_Perito.Items.Insert(0, ("<SELECCIONAR>"));
            Cmb_Perito.SelectedIndex = 0;


            //consultar tramites
            //Negocio_Consultar_Tramites.P_Area_Dependencia = Area_Dependencia;
            Dt_Tramites = Negocio_Consultar_Tramites.Consultar_Tabla_Tramite();


            Obj_Parametros.Consultar_Parametros();
            // validar que la consulta haya regresado valor
            if (!string.IsNullOrEmpty(Obj_Parametros.P_Dependencia_ID_Ordenamiento))
                Dependencia_ID_Ordenamiento = Obj_Parametros.P_Dependencia_ID_Ordenamiento;

            // validar que la consulta haya regresado valor
            if (!string.IsNullOrEmpty(Obj_Parametros.P_Dependencia_ID_Ambiental))
                Dependencia_ID_Ambiental = Obj_Parametros.P_Dependencia_ID_Ambiental;

            // validar que la consulta haya regresado valor
            if (!string.IsNullOrEmpty(Obj_Parametros.P_Dependencia_ID_Urbanistico))
                Dependencia_ID_Urbanistico = Obj_Parametros.P_Dependencia_ID_Urbanistico;

            // validar que la consulta haya regresado valor
            if (!string.IsNullOrEmpty(Obj_Parametros.P_Dependencia_ID_Inmobiliario))
                Dependencia_ID_Inmobiliario = Obj_Parametros.P_Dependencia_ID_Inmobiliario;


            //  filtro para obtener las areas de los parametros de ordenamiento
            Expresion_Sql.Append(Cat_Dependencias.Campo_Dependencia_ID + "='" + Dependencia_ID_Ordenamiento + "'");
            Expresion_Sql.Append(" or " + Cat_Dependencias.Campo_Dependencia_ID + "='" + Dependencia_ID_Ambiental + "'");
            Expresion_Sql.Append(" or " + Cat_Dependencias.Campo_Dependencia_ID + "='" + Dependencia_ID_Urbanistico + "'");
            Expresion_Sql.Append(" or " + Cat_Dependencias.Campo_Dependencia_ID + "='" + Dependencia_ID_Inmobiliario + "'");


            DataRow[] Drow_Dependencias_Ordenamiento = Dt_Tramites.Select(Expresion_Sql.ToString());

            Dt_Tramites = (DataTable)(Drow_Dependencias_Ordenamiento.CopyToDataTable());

            //  se ordenara la tabla por fecha
            DataView Dv_Ordenar = new DataView(Dt_Tramites);
            Dv_Ordenar.Sort = Cat_Tra_Tramites.Campo_Nombre;
            Dt_Tramites = Dv_Ordenar.ToTable();

            Cmb_Agregar_Solicitud.DataSource = Dt_Tramites;
            Cmb_Agregar_Solicitud.DataTextField = Cat_Tra_Tramites.Campo_Nombre;
            Cmb_Agregar_Solicitud.DataValueField = Cat_Tra_Tramites.Campo_Tramite_ID;
            Cmb_Agregar_Solicitud.DataBind();
        }
        catch (Exception ex)
        {
            throw new Exception("Llenar_Combos " + ex.Message);
        }
    }

    #endregion

    #region Validaciones

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Evaluacion
    ///DESCRIPCIÓN: Valida que antes de Evaluar la Solicitud, se contengan todos los
    ///             datos necesarios.
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 18/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///******************************************************************************* 
    private Boolean Validar_Evaluacion()
    {
        Boolean Completo = true;
        Boolean Consecutivo = true;
        String Mensaje_Error = "";
        Cls_Ope_Bandeja_Tramites_Negocio Negocio_Consecutivo = new Cls_Ope_Bandeja_Tramites_Negocio();


        Lbl_Mensaje_Error.Text = (Mensaje_Error);
        Div_Contenedor_Msj_Error.Visible = false;

        Lbl_Mensaje_Error.Text = "Para terminar de evaluar la Solicitud.";

        if (Txt_Costo_Total.Text == "")
        {
            Mensaje_Error = Mensaje_Error + "+ Faltan ingresar el costo del tramite.<br />";
            Completo = false;
        }
        if (Cmb_Evaluacion.SelectedItem.Value.Equals("APROBAR"))
        {
            if (!Validar_Plantillas_Llenas())
            {
                Mensaje_Error = Mensaje_Error + "+ Faltan Plantillas de Llenar.<br />";
                Completo = false;
            }
            if (!Validar_Formatos_Llenas())
            {
                Mensaje_Error = Mensaje_Error + "+ Faltan Formatos por Llenar.<br />";
                Completo = false;
            }

            // validar si el subproceso es de tipo COBRO, no permitir aplicar cambios
            DataTable Dt_Subproceso;
            Cls_Cat_Tramites_Negocio Neg_Consulta_Subprocesos = new Cls_Cat_Tramites_Negocio();
            Neg_Consulta_Subprocesos.P_Sub_Proceso_ID = Hdf_Subproceso_ID.Value.Trim();
            Dt_Subproceso = Neg_Consulta_Subprocesos.Consultar_Subprocesos_Tramite();
            // validar que la consulta haya regresado resultados
            if (Dt_Subproceso != null && Dt_Subproceso.Rows.Count > 0)
            {
                // si el subproceso es de tipo COBRO, no permitir guardar (no se puede APROBAR, sólo CANCELAR y DETENER)
                foreach (DataRow Fila_Subproceso in Dt_Subproceso.Rows)
                {
                    if (Fila_Subproceso[Cat_Tra_Subprocesos.Campo_Tipo_Actividad].ToString() == "COBRO")
                    {
                        Mensaje_Error = Mensaje_Error + "+ No es posible APROBAR la solicitud porque está pendiente de pago.<br />";
                        Completo = false;
                        break;
                    }
                }
            }
        }

        if (Cmb_Evaluacion.SelectedItem.Value.Equals("DETENER") || Cmb_Evaluacion.SelectedItem.Value.Equals("CANCELAR"))
        {
            Negocio_Consecutivo.P_Solicitud_ID = HDN_Solicitud_ID.Value;
            Negocio_Consecutivo = Negocio_Consecutivo.Consultar_Datos_Solicitud(); // Se obtienen los Datos a Detalle de la Solicitud Seleccionada

            //  si la solicitud tiene un consecutivo de ordenamiento no podra darse de baja (pedido por el usuario)
            if (Cmb_Evaluacion.SelectedItem.Value.Equals("CANCELAR"))
            {
                if (!String.IsNullOrEmpty(Negocio_Consecutivo.P_Consecutivo))
                {
                    Mensaje_Error = Mensaje_Error + "+ No puede realizar la operacion ya que la solicitud cuenta con un consecutivo.<br />";
                    Completo = false;
                    Consecutivo = false;
                }
            }
            if (Consecutivo != false)
            {
                if (Txt_Comentarios_Evaluacion.Text.Trim().Length == 0)
                {
                    Mensaje_Error = Mensaje_Error + "+ Falta Introducir los Comentarios referentes al ciudadano.<br />";
                    Completo = false;
                }
            }
        }

        //  validacion para el regreso cuando sea Pendiente
        if (Cmb_Evaluacion.SelectedValue == "REGRESAR" && Txt_Estatus.Text == "PENDIENTE")
        {
            Mensaje_Error = Mensaje_Error + "+ No se puede regresar ya que esta es la primera actividad del proceso de la solicitud.<br />";
            Completo = false;
        }

        //  validacion para no regresar a la actividad con estatus de cobro
        Cls_Ope_Bandeja_Tramites_Negocio Solicitud = new Cls_Ope_Bandeja_Tramites_Negocio();
        Cls_Ope_Bandeja_Tramites_Negocio Negocio_Datos_Solicitud = new Cls_Ope_Bandeja_Tramites_Negocio();
        Cls_Ope_Bandeja_Tramites_Negocio Negocio_Tipo_Actividad= new Cls_Ope_Bandeja_Tramites_Negocio();
        String Tipo_Actividad = "";
        String Orden_Actividad="";
        Boolean Tipo_Actividad_Cobro = false;
        //Boolean Realizar_Consulta = false;
        try
        {
            if (Cmb_Evaluacion.SelectedValue.Equals("REGRESAR") && Txt_Estatus.Text != "PENDIENTE")
            {
                Solicitud.P_Tipo_DataTable = "ACTUALIZACION_SOLICITUD";
                Solicitud.P_Solicitud_ID = Hdf_Solicitud_ID.Value;
                DataTable Subprocesos = Solicitud.Consultar_DataTable();

                Negocio_Datos_Solicitud.P_Solicitud_ID = Hdf_Solicitud_ID.Value;
                Negocio_Datos_Solicitud = Negocio_Datos_Solicitud.Consultar_Datos_Solicitud();

                //  si la variable no contiene la actividad anterior se busca cual es la anterior
                if (!String.IsNullOrEmpty(Negocio_Datos_Solicitud.P_SubProceso_Anterior))
                {
                    //  se comenzara de la ultima actividad a la primera
                    for (Int32 Contador = Subprocesos.Rows.Count - 1; Contador >= 0; Contador--)
                    {
                        if (Negocio_Datos_Solicitud.P_SubProceso_Anterior.Equals(Subprocesos.Rows[Contador][0].ToString()))
                        {
                            //  sse obtiene el id de la actividad anterior
                            Tipo_Actividad = Subprocesos.Rows[Contador][3].ToString();
                            Orden_Actividad = Subprocesos.Rows[Contador][2].ToString();
                            break;
                        }
                    }
                }
                else
                {
                    for (Int32 Contador = Subprocesos.Rows.Count - 1; Contador >= 0; Contador--)
                    {
                        if (Negocio_Datos_Solicitud.P_Subproceso_ID.Equals(Subprocesos.Rows[Contador][0].ToString()))
                        {
                            //  sse obtiene el id de la actividad anterior
                            Tipo_Actividad = Subprocesos.Rows[Contador - 1][3].ToString();
                            Orden_Actividad = Subprocesos.Rows[Contador][2].ToString();
                            break;
                        }
                    }
                }

                if (!String.IsNullOrEmpty(Orden_Actividad))
                {
                    Negocio_Datos_Solicitud.P_Orden = Orden_Actividad;
                    Negocio_Datos_Solicitud.P_Tramite_id = Hdf_Tramite_Id.Value;
                    DataTable Dt_Actividad_Condicion = Negocio_Datos_Solicitud.Consultar_Tipo_Actividad();
                    DataTable Dt_Tipo_Actividad = new DataTable();

                    if (Dt_Actividad_Condicion != null && Dt_Actividad_Condicion.Rows.Count > 0)
                    {
                        foreach (DataRow Registro in Dt_Actividad_Condicion.Rows)
                        {
                            for (int Contador_For = 0; Contador_For < 3; Contador_For++)
                            {
                                Negocio_Tipo_Actividad.P_Tramite_id = Hdf_Tramite_Id.Value;
                                
                                // validacino para el numero de la orden de la actividad
                                if (Contador_For == 0)
                                {
                                    if (!String.IsNullOrEmpty(Registro[Cat_Tra_Subprocesos.Campo_Orden].ToString()))
                                    {
                                        Negocio_Tipo_Actividad.P_Orden = Registro[Cat_Tra_Subprocesos.Campo_Orden].ToString();
                                        //Realizar_Consulta = true;
                                    }

                                    //else
                                    //    Realizar_Consulta = false;
                                }

                                //// validacino para la condicion si
                                //else if (Contador_For == 1)
                                //{
                                //    if (!String.IsNullOrEmpty(Registro[Cat_Tra_Subprocesos.Campo_Condicion_Si].ToString()))
                                //    {
                                //        Negocio_Tipo_Actividad.P_Orden = Registro[Cat_Tra_Subprocesos.Campo_Condicion_Si].ToString();
                                //        Realizar_Consulta = true;
                                //    }

                                //    else
                                //        Realizar_Consulta = false;
                                //}

                                //// validacino para la condicion si
                                //else if (Contador_For == 2)
                                //{
                                //    if (!String.IsNullOrEmpty(Registro[Cat_Tra_Subprocesos.Campo_Condicion_No].ToString()))
                                //    {
                                //        Negocio_Tipo_Actividad.P_Orden = Registro[Cat_Tra_Subprocesos.Campo_Condicion_No].ToString();
                                //        Realizar_Consulta = true;
                                //    }

                                //    else
                                //        Realizar_Consulta = false;
                                //}

                                ////  validacion para realizar la consulta
                                //if (Realizar_Consulta == true)
                                //{
                                //    Dt_Tipo_Actividad = Negocio_Tipo_Actividad.Consultar_Valor_Subproceso_ID();

                                //    if (Dt_Tipo_Actividad != null && Dt_Tipo_Actividad.Rows.Count > 0)
                                //    {
                                //        if (Dt_Tipo_Actividad.Rows[0][Cat_Tra_Subprocesos.Campo_Tipo_Actividad].ToString() == "COBRO")
                                //        {
                                //            Tipo_Actividad = "COBRO";
                                //            Tipo_Actividad_Cobro = true;
                                //            break;
                                //        }
                                //    }
                                //}
                            }

                            if (Tipo_Actividad_Cobro == true)
                                break;
                        }
                    }
                }


                if (Tipo_Actividad == "COBRO")
                {
                    Mensaje_Error = Mensaje_Error + "+ No se puede regresar a una actividad anterior ya que el ciudadano ya realizo su pago .<br/>";
                    Completo = false;
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Validar_Datos_Grid_Datos " + ex.Message.ToString(), ex);
        }


        // si los controles para mostrar zona están visibles (es un trámite de ordenamiento), validar selección de zona y supervisor
        if (Pnl_Contenedor_Zona.Style.Value == "display: block")
        {
            if (Cmb_Zonas.SelectedIndex <= 0)
            {
                Mensaje_Error += "+ Falta seleccionar una Zona.<br />";
                Completo = false;
            }
            if (Cmb_Supervisor_Zona.SelectedIndex <= 0)
            {
                Mensaje_Error += "+ Falta seleccionar un supervisor de Zona.<br />";
                Completo = false;
            }
        }

        if (!Completo)
        {
            Lbl_Mensaje_Error.Text = (Mensaje_Error);
            Div_Contenedor_Msj_Error.Visible = true;
        }
        return Completo;
    }
    
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Verificar_Fecha
    ///DESCRIPCIÓN: Metodo que permite generar la cadena de la fecha y valida las fechas ingresadas por el usuario
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 1/Septiembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public Boolean Verificar_Fecha()
    {
        DateTime Date1 = new DateTime();
        DateTime Date2 = new DateTime();
        Boolean Estatus_Operacion = true;
        try
        {
            //Variables que serviran para hacer la convecion a datetime las fechas y poder validarlas 
            if ((Txt_Fecha_Inicio.Text != "") && (Txt_Fecha_Fin.Text != ""))
            {
                //Convertimos el Texto de los TextBox fecha a dateTime
                Date1 = DateTime.Parse(Txt_Fecha_Inicio.Text);
                Date2 = DateTime.Parse(Txt_Fecha_Fin.Text);
                
                //Validamos que las fechas sean iguales o la final sea mayor que la inicias, si no se manda un mensaje de error 
                if ((Date1 < Date2) | (Date1 == Date2))
                    Estatus_Operacion = true;
               
                else
                {
                    Lbl_Mensaje_Error.Text += "+ Fecha no valida (Fecha inicial mayor a la final) <br />";
                    Div_Contenedor_Msj_Error.Visible = true;
                    Estatus_Operacion = false;
                }
            }
            else
            {
                Lbl_Mensaje_Error.Text += "+ selecciones la fecha";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
        return Estatus_Operacion;
    }
    
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Validar_Datos_Grid_Datos
    /// DESCRIPCION : Validar que se se encuentre todos los datos para continuar con el proceso
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 03/Mayo/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private Boolean Validar_Datos_Grid_Datos()
    {
        String Espacios_Blanco = "";
        Boolean Datos_Validos = true;//Variable que almacena el valor de true si todos los datos fueron ingresados de forma correcta, o false en caso contrario.
        
        DataTable Dt_Datos = (DataTable)(Session["Grid_Datos"]);
        try
        {

            Espacios_Blanco = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
            Lbl_Mensaje_Error.Text += Espacios_Blanco + "Es necesario Introducir: <br />";
            Lbl_Mensaje_Error.Visible = true;
            IBtn_Imagen_Error.Visible = true;


            //  saca las dimenciones del arreglo
            String[,] Datos = new String[Dt_Datos.Rows.Count, 2];

            //  para saber si cuenta con informacion 
            for (int Contador_For = 0; Contador_For < Dt_Datos.Rows.Count; Contador_For++)
            {
                String Valor_Dato = ((TextBox)Grid_Datos_Dictamen.Rows[Contador_For].FindControl("Txt_Descripcion_Datos")).Text;

                if (Valor_Dato != "" || (Dt_Datos.Rows[Contador_For][Cat_Tra_Datos_Tramite.Campo_Dato_Requerido].ToString()) == "N")
                {
                }

                else
                {
                    Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Ingrese el dato de " +
                            Dt_Datos.Rows[Contador_For][Cat_Tra_Datos_Tramite.Campo_Nombre] + ".<br />";
                    Datos_Validos = false;
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Validar_Datos_Grid_Datos " + ex.Message.ToString(), ex);
        }

        return Datos_Validos;
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Validar_Datos_Grid_Datos_Modificar
    /// DESCRIPCION : Validar que se se encuentre todos los datos para continuar con el proceso
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 03/Mayo/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private Boolean Validar_Datos_Grid_Datos_Modificar()
    {
        String Espacios_Blanco = "";
        Boolean Datos_Validos = true;//Variable que almacena el valor de true si todos los datos fueron ingresados de forma correcta, o false en caso contrario.

        DataTable Dt_Datos_Modificar = (DataTable)(Session["Grid_Datos_Modificar"]);
        try
        {

            Espacios_Blanco = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
            Lbl_Mensaje_Error.Text += Espacios_Blanco + "Es necesario Introducir: <br />";
            Lbl_Mensaje_Error.Visible = true;
            IBtn_Imagen_Error.Visible = true;


            //  saca las dimenciones del arreglo
            String[,] Datos = new String[Dt_Datos_Modificar.Rows.Count, 2];

            //  para saber si cuenta con informacion 
            for (int Contador_For = 0; Contador_For < Dt_Datos_Modificar.Rows.Count; Contador_For++)
            {
                String Valor_Dato = ((TextBox)Grid_Datos_Dictamen_Modificar.Rows[Contador_For].FindControl("Txt_Valor_Dato")).Text;
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Validar_Datos_Grid_Datos " + ex.Message.ToString(), ex);
        }

        return Datos_Validos;
    }
    
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Plantillas_Llenas
    ///DESCRIPCIÓN: Valida que las plantillas hayan sido terminadas.
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 18/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///******************************************************************************* 
    private Boolean Validar_Plantillas_Llenas()
    {
        Boolean Plantillas_Completas = false;
        try
        {
            if (Grid_Plantillas.Rows.Count > 0)
            {
                for (Int32 Contador = 0; Contador < Grid_Plantillas.Rows.Count; Contador++)
                {
                    System.Web.UI.WebControls.CheckBox Check_Temporal = (System.Web.UI.WebControls.CheckBox)Grid_Plantillas.Rows[Contador].FindControl("Chk_Realizado");
                    if (Check_Temporal.Checked)
                    {
                        Plantillas_Completas = true;
                        break;
                    }
                }
            }
            else
            {
                Plantillas_Completas = true;
            }
        }
        catch (Exception ex)
        {
            Mensaje_Error(true, "Validar_Plantillas_Llenas: " + ex.Message.ToString());
        }

        return Plantillas_Completas;
    }
    
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Validar_Formatos_Llenas
    ///DESCRIPCIÓN          : Metodo que valida los formatos
    ///PARAMETROS           :
    ///CREO                 : Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO           : 08/Junio/2012
    ///MODIFICO             :
    ///FECHA_MODIFICO       :
    ///CAUSA_MODIFICACIÓN   :
    ///*******************************************************************************
    private Boolean Validar_Formatos_Llenas()
    {
        Boolean Plantillas_Completas = true;
        try
        {
            for (Int32 Contador = 0; Contador < Grid_Detalles_Formatos.Rows.Count; Contador++)
            {
                System.Web.UI.WebControls.CheckBox Check_Temporal = (System.Web.UI.WebControls.CheckBox)Grid_Detalles_Formatos.Rows[Contador].FindControl("Chk_Realizado");
                if (!Check_Temporal.Checked)
                {
                    Plantillas_Completas = false;
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            Mensaje_Error(true, "Validar_Formatos_Llenas: " + ex.Message.ToString());
        }
        return Plantillas_Completas;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Plantilla
    ///DESCRIPCIÓN: Valida que todos los campos de la platilla fueron llenados.
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 18/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///******************************************************************************* 
    private Boolean Validar_Plantilla()
    {
        Boolean Plantilla_Completa = true;
        try
        {
            for (Int32 Contador = 0; Contador < Grid_Marcadores_Platilla.Rows.Count; Contador++)
            {
                TextBox Text_Temporal = (TextBox)Grid_Marcadores_Platilla.Rows[Contador].FindControl("Txt_Valor_Marcador");
                if (Text_Temporal.Text.Trim().Length == 0)
                {
                    Plantilla_Completa = false;
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            Mensaje_Error(true, "Validar_Plantilla: " + ex.Message.ToString());
        }
        return Plantilla_Completa;
    }

    #endregion

    #endregion

    #region Grids

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Bandeja_Entrada_PageIndexChanging
    ///DESCRIPCIÓN: Maneja la paginación del Grid de Bandeja de Entrada.
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 15/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///******************************************************************************* 
    protected void Grid_Bandeja_Entrada_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Llenar_Grid_Solicitudes_Tramites(e.NewPageIndex);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }

    ///********************************************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Grid_Bandeja_Entrada_Sorting
    ///DESCRIPCIÓN          : ordena las columnas
    ///PROPIEDADES          :
    ///CREO                 : Hugo Enrique Ramirez Aguilera
    /// FECHA_CREO          : 28/Junio/2012
    ///MODIFICO             :
    ///FECHA_MODIFICO       :
    ///CAUSA_MODIFICACIÓN...:
    ///*********************************************************************************************************
    protected void Grid_Bandeja_Entrada_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable Dt_Consulta = (DataTable)(Session["GRID_BANDEJA_TRAMITES"]);

        DataView Dv_Ordenar = new DataView(Dt_Consulta);
        String Orden = ViewState["SortDirection"].ToString();

        if (Orden.Equals("ASC"))
        {
            Dv_Ordenar.Sort = e.SortExpression + " " + "DESC";
            ViewState["SortDirection"] = "DESC";
        }
        else
        {
            Dv_Ordenar.Sort = e.SortExpression + " " + "ASC";
            ViewState["SortDirection"] = "ASC";
        }
        Grid_Bandeja_Entrada.Columns[1].Visible = true;
        Grid_Bandeja_Entrada.DataSource = Dv_Ordenar;
        Grid_Bandeja_Entrada.DataBind();
        Grid_Bandeja_Entrada.Columns[1].Visible = false;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Bandeja_Entrada_PageIndexChanging
    ///DESCRIPCIÓN: Maneja el Evento del Cambio de Selección.
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 15/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///******************************************************************************* 
    protected void Grid_Bandeja_Entrada_SelectedIndexChanged(object sender, EventArgs e)
    {
        String Elemento_ID = ""; // Se obtiene el ID de la Solicitud Seleccionada.
        Cls_Ope_Bandeja_Tramites_Negocio Solicitud = new Cls_Ope_Bandeja_Tramites_Negocio();
        Cls_Cat_Tramites_Negocio Negocio_Subproceso = new Cls_Cat_Tramites_Negocio();
        Cls_Ope_Bandeja_Tramites_Negocio Negocio_Plantillas_Formatos = new Cls_Ope_Bandeja_Tramites_Negocio();
        Cls_Rpt_Ven_Consultar_Tramites_Negocio Negocio_Actividades_Realizadas = new Cls_Rpt_Ven_Consultar_Tramites_Negocio();
        Cls_Cat_Ort_Parametros_Negocio Obj_Parametros = new Cls_Cat_Ort_Parametros_Negocio();
        DataTable Dt_Valor_Subproceso = new DataTable();
        DataTable Dt_Detalles_Plantillas = new DataTable();
        DataTable Dt_Detalles_Formato = new DataTable();
        DataTable Dt_Actividades_Realizadas = new DataTable();        
        DataTable Dt_Comentarios_Internos = new DataTable();
        DataTable Dt_Siguiente_Actividad = new DataTable();
        Double Porcentaje = 0.0;
        String Tramite_ID = "";
        String Orden_Actividad = "";
        string Dependencia_ID_Ordenamiento = "";
        string Dependencia_ID_Ambiental = "";
        string Dependencia_ID_Urbanistico = "";
        string Dependencia_ID_Inmobiliario = ""; 
        string Dependencia_ID_Catastro = "";
        String Rol_Director_Ordenamiento = "";
        String Rol_Director_Ambiental = "";
        String Rol_Director_Fraccionamientos = "";
        String Rol_Director_Urbano = "";
        Boolean Bloqueo_Ficha = false;

        try
        {
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = false;

            // consultar parámetros
            Obj_Parametros.Consultar_Parametros();

            //  valores de las dependencias de ordenamiento
            if (!string.IsNullOrEmpty(Obj_Parametros.P_Dependencia_ID_Ordenamiento))
                Dependencia_ID_Ordenamiento = Obj_Parametros.P_Dependencia_ID_Ordenamiento;

            if (!string.IsNullOrEmpty(Obj_Parametros.P_Dependencia_ID_Ambiental))
                Dependencia_ID_Ambiental = Obj_Parametros.P_Dependencia_ID_Ambiental;

            if (!string.IsNullOrEmpty(Obj_Parametros.P_Dependencia_ID_Urbanistico))
                Dependencia_ID_Urbanistico = Obj_Parametros.P_Dependencia_ID_Urbanistico;

            if (!string.IsNullOrEmpty(Obj_Parametros.P_Dependencia_ID_Inmobiliario))
                Dependencia_ID_Inmobiliario = Obj_Parametros.P_Dependencia_ID_Inmobiliario;

            if (!string.IsNullOrEmpty(Obj_Parametros.P_Dependencia_ID_Catastro))
                Dependencia_ID_Catastro = Obj_Parametros.P_Dependencia_ID_Catastro;
         
            //  roles de los directores de ordenamiento
            if (!string.IsNullOrEmpty(Obj_Parametros.P_Rol_Director_Ordenamiento))
                Rol_Director_Ordenamiento = Obj_Parametros.P_Rol_Director_Ordenamiento;

            if (!string.IsNullOrEmpty(Obj_Parametros.P_Rol_Director_Ambiental))
                Rol_Director_Ambiental = Obj_Parametros.P_Rol_Director_Ambiental;

            if (!string.IsNullOrEmpty(Obj_Parametros.P_Rol_Director_Fraccionamientos))
                Rol_Director_Fraccionamientos = Obj_Parametros.P_Rol_Director_Fraccionamientos;

            if (!string.IsNullOrEmpty(Obj_Parametros.P_Rol_Director_Urbana))
                Rol_Director_Urbano = Obj_Parametros.P_Rol_Director_Urbana;


            //  si pertenece la solicitud de otra pagina
            if (Request.QueryString["Solicitud"] != null || Session["Solicitud"] != null)
            {
                Elemento_ID = Hdf_Solicitud_ID.Value;
                Bloqueo_Ficha = true;
                if (Session["Solicitud"] != null)
                {
                    Bloqueo_Ficha = false;
                }
            }
            else
            {
                Elemento_ID = Grid_Bandeja_Entrada.SelectedRow.Cells[1].Text;
                Bloqueo_Ficha = false;
                Habilitar_Elementos_Revision_Ficha(true);
            }

            Limpiar_Formulario(); // Se limpia el Catalogo
            if (Grid_Bandeja_Entrada.SelectedIndex > (-1) || (Request.QueryString["Solicitud"] != null) || Session["Solicitud"] != null)
            {
                Pnl_Filtro_Fechas.Style.Value = "width:98%; display:none";

                //  se cargan los id
                Solicitud.P_Solicitud_ID = Elemento_ID;
                HDN_Solicitud_ID.Value = Elemento_ID;
                Solicitud = Solicitud.Consultar_Datos_Solicitud();


                //  habilita el boton del reporte de orden de pago
                if (Solicitud.P_Estatus == "PROCESO")
                {
                    Btn_Reporte_Orden_Pago.Visible = true;
                }
                else
                {
                    Btn_Reporte_Orden_Pago.Visible = false;
                }

                //Se cargan las solicitudes complementarias
                Grid_Solicitudes_Complemetarias.DataSource = Solicitud.P_Solicitudes_Complementarias;
                Grid_Solicitudes_Complemetarias.DataBind();

                //  se consultan si tiene alguna cedula de inspeccion
                DataTable Dt_Consulta_Cedula_Existente = Solicitud.Consultar_Datos_Cedula_Visita();

                if (Dt_Consulta_Cedula_Existente != null && Dt_Consulta_Cedula_Existente.Rows.Count > 0)
                    Btn_Link_Cedula_Visita.Visible = true;
                
                else
                    Btn_Link_Cedula_Visita.Visible = false;
                
                //  se consultan si tiene alguna ficha de revision
                DataTable Dt_Consulta_Ficha_Revision = Solicitud.Consultar_Datos_Ficha_Revision();
                if (Dt_Consulta_Ficha_Revision != null && Dt_Consulta_Ficha_Revision.Rows.Count > 0)
                    Btn_Link_Opiniones.Visible = true;

                else
                    Btn_Link_Opiniones.Visible = false;

                //  se muesta la seccion de detalles y se oculta el grid
                Ocultar_Div(true);

                //   se comienza a pasar la información de la capa de negocio
                Orden_Actividad = Solicitud.P_Orden; //  para el numero de la actividad
                Hdf_Solicitud_ID.Value = Solicitud.P_Solicitud_ID;
                Txt_Clave_Solicitud.Text = Solicitud.P_Clave_Solicitud;
                Txt_Porcentaje_Avance.Text = Solicitud.P_Porcentaje_Avance.ToString("#,###,###.00");
                Txt_Nombre_Tramite.Text = Solicitud.P_Tramite;
                Txt_Solicito.Text = Solicitud.P_Solicito;
                Hdf_Subproceso_ID.Value = Solicitud.P_Subproceso_ID;
                Tramite_ID = Solicitud.P_Tramite_id;
                Hdf_Tramite_Id.Value = Tramite_ID;
 
                //  se carga en consecutivo
                if (!String.IsNullOrEmpty(Solicitud.P_Consecutivo))
                {
                    Txt_Consecutivo.Text = !String.IsNullOrEmpty(Solicitud.P_Consecutivo.Trim())
                        ? Convert.ToInt64(Solicitud.P_Consecutivo.Trim()).ToString()
                        : !String.IsNullOrEmpty(Solicitud.P_Folio.Trim())
                        ? Solicitud.P_Folio.Trim().ToString()
                        : "";
                }


                if (!String.IsNullOrEmpty(Solicitud.P_Contribuyente_Id))
                    Hdf_Contribuyente_Id.Value = Solicitud.P_Contribuyente_Id;//    Se carga el id del contribuyente

                Txt_Costo_Total.Text = "" + Solicitud.P_Costo_Total;//  se llena la caja de texto del costo del tramite
                
                //  se consultan los datos finales del dictamen
                DataTable Dt_Consulta_Dato_Final = Solicitud.Consultar_Datos_Finales();
                
                //  para cuando tenga registros para el dictamen
                if (Dt_Consulta_Dato_Final != null && Dt_Consulta_Dato_Final.Rows.Count > 0)
                {
                    //  carga el grid con los datos a dictaminar
                    DataTable Dt_Consulta_Dato_Final_Llenado = Solicitud.Consultar_Datos_Finales_Operacion();
                    Cargar_Grid_Datos_Dictamen_Modificar(Dt_Consulta_Dato_Final_Llenado);

                    Grid_Datos_Dictamen.Visible = false;
                    Grid_Datos_Dictamen_Modificar.Visible = true;
                    Btn_Guardar_Datos_Dictamen.Visible = true;
                    Btn_Guardar_Datos_Dictamen.ToolTip = "Modificar datos a dictaminar";
                }
                else
                {
                    Cargar_Grid_Datos_Dictamen(Solicitud.P_Tramite_id);

                    Grid_Datos_Dictamen.Visible = true;
                    Grid_Datos_Dictamen_Modificar.Visible = false;
                    Btn_Guardar_Datos_Dictamen.Visible = true;
                    Btn_Guardar_Datos_Dictamen.ToolTip = "Guardar datos a dictaminar";
                }

                String Actividad_Perfil = "";
                String Actividad_ID = "";

                //  se carga el perfil de la siguiente actividad
                Dt_Siguiente_Actividad = Solicitud.Consultar_Siguiente_Actividad();
                if (Dt_Siguiente_Actividad != null && Dt_Siguiente_Actividad.Rows.Count > 0)
                {
                    foreach (DataRow Registro in Dt_Siguiente_Actividad.Rows)
                    {
                        if (Convert.ToDouble(Solicitud.P_Orden_Actividad) + 1 == Convert.ToDouble(Registro["ORDEN"].ToString()))
                        {
                            //  campos del siguinete perfil
                            Actividad_Perfil += Registro["NOMBRE_PERFIL"].ToString() + ", ";
                        }

                    }
                    Txt_Siguiente_Subproceso.Text = Dt_Siguiente_Actividad.Rows[0][Cat_Tra_Subprocesos.Campo_Nombre].ToString();
                    Actividad_ID = Dt_Siguiente_Actividad.Rows[0][Cat_Tra_Subprocesos.Campo_Subproceso_ID].ToString();
                    Txt_Nombre_Actividad_Condicion.Text = Dt_Siguiente_Actividad.Rows[0][Cat_Tra_Subprocesos.Campo_Nombre].ToString();
                    Txt_Siguiente_Subproceso.Visible = true;
                    Lbl_Siguiente_Subproceso.Visible = true;

                    Txt_Perfil.Text = Actividad_Perfil;
                    Txt_Perfil.Visible = true;
                    Lbl_Perfil.Visible = true;
                   
                }

                /*********************************** se calcula el costo de la solicitud ***********************************/
                Calcular_Costo(Actividad_ID, Solicitud);
                /***********************************************************************************************************/

                if (Solicitud.P_Complemento != "")
                {
                    TPnl_Agregar_Tramite.Visible = false;
                    Btn_Evaluar.Visible = false;
                }

                else
                {
                    TPnl_Agregar_Tramite.Visible = true;
                    Btn_Evaluar.Visible = true;
                }


                //  para el tramite_ID cuando la actividad sea condicion se habilita el combo de si o no
                if (Solicitud.P_Tipo_Actividad == "CONDICION")
                {
                    Btn_Guardar_Datos_Dictamen.Visible = true;

                    Tabla_Condicion.Style.Value = "display:block";
                    Lbl_Siguiente_Subproceso.Visible = false;
                    Txt_Siguiente_Subproceso.Visible = false;
                    Hdf_Condicion_Si.Value = "" + Solicitud.P_Condicion_Si;
                    Hdf_Condicion_No.Value = "" + Solicitud.P_Condicion_No;
                    Lbl_Perfil.Style.Value = "display: none";
                    Txt_Perfil.Style.Value = "display: none";
                    Cmb_Condicion_SelectedIndexChanged(sender, null);

                    //  muestra los campos del combo de evaluar
                    Mostrar_Componenetes_Evaluar(true);

                    //if (Txt_Siguiente_Subproceso.Text.ToUpper().Contains("PAGO"))
                      
                    Lbl_Condicion.Text = "¿ Requiere " + Txt_Siguiente_Subproceso.Text + " ?";
                }
                else if (Solicitud.P_Tipo_Actividad == "VALIDACION")
                {
                    Tabla_Condicion.Style.Value = "display:none";
                    Lbl_Perfil.Style.Value = "display: block";
                    Txt_Perfil.Style.Value = "display: block";


                    //Txt_Fecha_Vigencia_Inicio.Enabled = false;
                    //Txt_Fecha_Vigencia_Fin.Enabled = false;
                }
                else if (Solicitud.P_Tipo_Actividad == "ELABORAR" || Solicitud.P_Complemento != "")
                {
                    //  muestra los campos del combo de evaluar
                    Mostrar_Componenetes_Evaluar(true);

                    Btn_Guardar_Datos_Dictamen.Visible = true;
                    Tabla_Condicion.Style.Value = "display:none";
                    Lbl_Perfil.Style.Value = "display: block";
                    Txt_Perfil.Style.Value = "display: block";
                    Lbl_Siguiente_Subproceso.Visible = true;
                    Txt_Siguiente_Subproceso.Visible = true;
                }

                else if (Solicitud.P_Tipo_Actividad == "TERMINADO")
                {
                    Txt_Ubicacion_Expediente.Enabled = true;
                }
                else
                {
                    Mostrar_Componenetes_Evaluar(true);//  muestra los campos del combo de evaluar

                    Tabla_Condicion.Style.Value = "display:none";
                    Lbl_Siguiente_Subproceso.Visible = true;
                    Txt_Siguiente_Subproceso.Visible = true;
                    Btn_Guardar_Datos_Dictamen.Visible = false;
                    //Txt_Fecha_Vigencia_Inicio.Enabled = false;
                    //Txt_Fecha_Vigencia_Fin.Enabled = false;

                    //  para el nombre de la actividad siguiente a realizar
                    Solicitud.P_Orden = Orden_Actividad;
                    Dt_Siguiente_Actividad = Solicitud.Consultar_Siguiente_Actividad();
                    if (Dt_Siguiente_Actividad != null && Dt_Siguiente_Actividad.Rows.Count > 0)
                    {
                        Txt_Siguiente_Subproceso.Text = Dt_Siguiente_Actividad.Rows[0][Cat_Tra_Subprocesos.Campo_Nombre].ToString();
                    }
                    else
                    {
                        Txt_Siguiente_Subproceso.Text = "FIN DEL PROCESO DE LA SOLICITUD";
                        Txt_Perfil.Text = "FIN DEL PROCESO";
                    }
                    Hdf_Condicion_Si.Value = "0";
                    Hdf_Condicion_No.Value = "0";
                    Txt_Siguiente_Subproceso.Visible = true;
                    Lbl_Siguiente_Subproceso.Visible = true;
                }


                // si tiene asignada una dependencia y es un trámite de ordenamiento territorial, mostrar combos zona y supervisor
                if ((!string.IsNullOrEmpty(Solicitud.P_Dependencia_ID) && Solicitud.P_Dependencia_ID == Dependencia_ID_Ordenamiento)
                    || (!string.IsNullOrEmpty(Solicitud.P_Dependencia_ID) && Solicitud.P_Dependencia_ID == Dependencia_ID_Ambiental)
                    || (!string.IsNullOrEmpty(Solicitud.P_Dependencia_ID) && Solicitud.P_Dependencia_ID == Dependencia_ID_Inmobiliario)
                    || (!string.IsNullOrEmpty(Solicitud.P_Dependencia_ID) && Solicitud.P_Dependencia_ID == Dependencia_ID_Urbanistico)
                    || (!string.IsNullOrEmpty(Solicitud.P_Dependencia_ID) && Solicitud.P_Dependencia_ID == Dependencia_ID_Catastro))
                {
                    // se muestran los campos de la ubicacion del expediente
                    Tbl_Ubicacion_Expediente.Visible = true;

                    Tbl_Fechas_Vigencia.Visible = true;
                    //  validacion para el llenado de las fechas de vigencia
                    if ((Solicitud.P_Fecha_Date_Vigencia_Fin != DateTime.Today))
                    {
                        if ((Solicitud.P_Fecha_Date_Vigencia_Inicio != DateTime.Today))
                        {
                            Txt_Fecha_Vigencia_Inicio.Text = "" + String.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(Solicitud.P_Fecha_Date_Vigencia_Inicio));
                            Txt_Fecha_Vigencia_Fin.Text = "" + String.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(Solicitud.P_Fecha_Date_Vigencia_Fin));
                        }
                        else
                        {
                            Txt_Fecha_Vigencia_Inicio.Text = "";
                            Txt_Fecha_Vigencia_Fin.Text = "";
                        }
                    }
                    else
                    {
                        Txt_Fecha_Vigencia_Inicio.Text = ""; 
                        Txt_Fecha_Vigencia_Fin.Text = "";
                    }

                    //  validacion para el llenado de las fechas del documento con vigencia
                    if ((Solicitud.P_Date_Fecha_Documento_Vigencia_Fin != DateTime.Today))
                    {
                        if ((Solicitud.P_Date_Fecha_Documento_Vigencia_Inicio != DateTime.Today))
                        {
                            Txt_Fecha_Doc_Inicio.Text = "" + String.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(Solicitud.P_Date_Fecha_Documento_Vigencia_Inicio));
                            Txt_Fecha_Doc_Fin.Text = "" + String.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(Solicitud.P_Date_Fecha_Documento_Vigencia_Fin));
                        }
                        else
                        {
                            Txt_Fecha_Doc_Inicio.Text = "";
                            Txt_Fecha_Doc_Fin.Text = "";
                        }
                    }
                    else
                    {
                        Txt_Fecha_Doc_Inicio.Text = ""; 
                        Txt_Fecha_Doc_Fin.Text = "";
                    }



                    //  validacion para la dependencia de catastro dashabilita las zonas
                    if (Solicitud.P_Dependencia_ID == Dependencia_ID_Catastro)
                        Pnl_Contenedor_Zona.Style.Value = "display:none";

                    //  habilita las zonas
                    else
                        Pnl_Contenedor_Zona.Style.Value = "display:block";

                    //  se cargaran las zonas correspondientes al tramite
                    Llenar_Combos(Solicitud.P_Area_Dependencia);
                }
                else
                {
                    Tbl_Fechas_Vigencia.Visible = false;
                    Pnl_Contenedor_Zona.Style.Value = "display:none";
                    Txt_Fecha_Vigencia_Inicio.Text = "";
                    Txt_Fecha_Vigencia_Fin.Text = "";
                }
                // si hay una zona cargar en el combo correspondiente
                if (!string.IsNullOrEmpty(Solicitud.P_Zona_ID))
                {
                    if (Cmb_Zonas.Items.FindByValue(Solicitud.P_Zona_ID) != null)
                    {
                        Cmb_Zonas.SelectedValue = Solicitud.P_Zona_ID;
                        Cmb_Zonas.Enabled = false;
                    }
                }
                else
                    Cmb_Zonas.Enabled = true;
               
                // empleado_id cargar en el combo correspondiente
                if (!string.IsNullOrEmpty(Solicitud.P_Empleado_ID))
                {
                    if (Cmb_Supervisor_Zona.Items.FindByValue(Solicitud.P_Empleado_ID) != null)
                    {
                        Cmb_Supervisor_Zona.SelectedValue = Solicitud.P_Empleado_ID;
                        Cmb_Supervisor_Zona.Enabled = false;
                    }
                }
                else
                {
                    if (Solicitud.P_Estatus == "PENDIENTE")
                    {
                        Cmb_Zonas.Enabled = true;
                        Cmb_Supervisor_Zona.Enabled = false;
                    }
                    else
                        Cmb_Supervisor_Zona.Enabled = true;
                }

                // si hay una zona cargar en el combo correspondiente
                if (!string.IsNullOrEmpty(Solicitud.P_Persona_Inspecciona))
                {
                    if (Cmb_Inspector.Items.FindByValue(Solicitud.P_Persona_Inspecciona) != null)
                    {
                        Cmb_Inspector.SelectedValue = Solicitud.P_Persona_Inspecciona;
                        Cmb_Inspector.Enabled = false;
                    }
                }
                else
                {
                    Cmb_Inspector.Enabled = true;
                    Cargar_Combo_Nombres();
                }


                //  validacion para el director el cual podra cambiar la zona
                if (Rol_Director_Ordenamiento == Cls_Sessiones.Rol_ID ||
                    Rol_Director_Ambiental == Cls_Sessiones.Rol_ID ||
                    Rol_Director_Fraccionamientos == Cls_Sessiones.Rol_ID ||
                    Rol_Director_Urbano == Cls_Sessiones.Rol_ID)
                   
                {
                    Cmb_Zonas.Enabled = false;
                    Cmb_Supervisor_Zona.Enabled = true;
                    Btn_Modificar.Enabled = true;
                }
                else
                    Btn_Modificar.Enabled = false;


                //  para la cuenta predial
                if (!string.IsNullOrEmpty(Solicitud.P_Cuenta_Predial))
                {
                    Lbl_Cuenta_Predial.Text = "Cuenta Predial";
                    Txt_Cuenta_Predial.Text = Solicitud.P_Cuenta_Predial;
                    String Cuenta_Predial_Id = Consultar_Cuenta_Predial_ID(Txt_Cuenta_Predial.Text.Trim().ToUpper());
                    Btn_Buscar_Cuenta_Predial.Enabled = true;
                    Cargar_Ventana_Emergente_Resumen_Predio();
                    Cls_Sessiones.Ciudadano_ID = Hdf_Solicitud_ID.Value;

                    //  se saca el nombre del propietario de la cuenta predial
                    Txt_Propietario_Cuenta_Predial.Text = Consultar_Propietario(Cuenta_Predial_Id);
                    Txt_Direccion_Predio.Text = Solicitud.P_Direccion_Predio;
                    Txt_Calle_Predio.Text = Solicitud.P_Calle_Predio;
                    Txt_Numero_Predio.Text = Solicitud.P_Nuemro_Predio;
                    Txt_Manzana_Predio.Text = Solicitud.P_Manzana_Predio;
                    Txt_Lote_Predio.Text = Solicitud.P_Lote_Predio;
                    Txt_Otros_Predio.Text = Solicitud.P_Otros_Predio;
                }
                else if ((!string.IsNullOrEmpty(Solicitud.P_Direccion_Predio) && (!string.IsNullOrEmpty(Solicitud.P_Propietario_Predio))))
                {
                    Txt_Propietario_Cuenta_Predial.Text = Solicitud.P_Propietario_Predio;
                    Txt_Direccion_Predio.Text = Solicitud.P_Direccion_Predio;
                    Txt_Calle_Predio.Text = Solicitud.P_Calle_Predio;
                    Txt_Numero_Predio.Text = Solicitud.P_Nuemro_Predio;
                    Txt_Manzana_Predio.Text = Solicitud.P_Manzana_Predio;
                    Txt_Lote_Predio.Text = Solicitud.P_Lote_Predio;
                    Btn_Buscar_Cuenta_Predial.Enabled = false;
                    Txt_Otros_Predio.Text = Solicitud.P_Otros_Predio;
                }

                else
                {
                    Txt_Cuenta_Predial.Text = "";
                    Cargar_Ventana_Emergente_Resumen_Predio();
                    Txt_Propietario_Cuenta_Predial.Text = "";

                    if (Cmb_Perito.Items.Count > 0) 
                        Cmb_Perito.SelectedIndex = 0;
                }

                //  se carga el perito
                if (!string.IsNullOrEmpty(Solicitud.P_Inspector_ID))
                {
                    if (Cmb_Perito.Items.FindByValue(Solicitud.P_Inspector_ID) != null)
                    {
                        Cmb_Perito.SelectedValue = Solicitud.P_Inspector_ID;
                        Cmb_Perito.Visible = true;
                        Lbl_Perito.Visible = true;
                    }

                }
                else
                {
                    if (Cmb_Perito.Items.Count > 0)
                    {
                        Cmb_Perito.SelectedIndex = 1;
                        Cmb_Perito.Visible = false;
                        Lbl_Perito.Visible = false;
                    }
                }

                //  se realiza la consulta de las plantillas y formatos
                Negocio_Plantillas_Formatos.P_Subproceso_ID = Solicitud.P_Subproceso_ID;
                Dt_Detalles_Plantillas = Negocio_Plantillas_Formatos.Consultar_Detalles_Plantillas();
                Dt_Detalles_Formato = Negocio_Plantillas_Formatos.Consultar_Detalles_Formatos();

                //  se consulta el valor % del proceso que se esta realizando
                Solicitud.P_Orden = String.Empty;
                Dt_Valor_Subproceso = Solicitud.Consultar_Valor_Subproceso_ID();
                if (Dt_Valor_Subproceso is DataTable)
                {
                    if (Dt_Valor_Subproceso.Rows.Count > 0)
                    {
                        Porcentaje = Convert.ToDouble(Dt_Valor_Subproceso.Rows[0][Cat_Tra_Subprocesos.Campo_Valor].ToString());
                        Txt_Porcentaje_Actual_Proceso.Text = Porcentaje.ToString("#,###,###.00");
                    }
                }

                Txt_Subproceso.Text = Solicitud.P_Subproceso_Nombre;
                Txt_Estatus.Text = Solicitud.P_Estatus;

                if (Solicitud.P_Fecha_Solicitud != null)
                    Txt_Fecha_Solicitud.Text = "" + Solicitud.P_Fecha_Solicitud;

                //  se llenan los grids
                Llenar_Grid_Datos_Solicitud(Solicitud.P_Datos_Solicitud);
                Llenar_Grid_Documentacion_Solicitud(Solicitud.P_Documentos_Solicitud);
                Llenar_Grid_Platillas_Subproceso(Dt_Detalles_Plantillas);
                Llenar_Grid_Formatos_Subproceso(Dt_Detalles_Formato);

                Cargar_Documentos_Seguimiento(Solicitud, Rol_Director_Ordenamiento, Rol_Director_Ambiental, Rol_Director_Fraccionamientos, Rol_Director_Urbano);//  se cargan los archivos del seguimiento de la solicitud
                AFU_Subir_Archivo.Enabled = true;

                //  para las actividades realizadas
                Negocio_Actividades_Realizadas.P_Solicitud_id = Elemento_ID;
                Dt_Actividades_Realizadas = Negocio_Actividades_Realizadas.Consultar_Historial_Actividades();
                Cargar_Grid_Actividades(Dt_Actividades_Realizadas);

                //  para los comentarios internos
                Dt_Comentarios_Internos = Solicitud.Consultar_Comentarios_Internos();
                Cargar_Grid_Comentarios(Dt_Comentarios_Internos);

                //  se consulta la información de las plantillas
                Solicitud.P_Tipo_DataTable = "FUENTE_DATOS_PLANTILLAS";
                DataTable Temporal = Solicitud.Consultar_DataTable();
                
                if (Temporal != null && Temporal.Rows.Count > 0)
                    Session["Dt_Fuente_Datos"] = Temporal;
               
                Configuracion_Catalogo(true);
                Habilitar_Comentarios(true);

                if (Bloqueo_Ficha == true)
                {
                    Configuracion_Catalogo(false);
                    Habilitar_Elementos_Revision_Ficha(false);
                    Cls_Sessiones.Ciudadano_ID = "";
                }
            }
            else
            {
                Configuracion_Catalogo(false);
                Habilitar_Comentarios(false);
            }

            ////  para habilitar el boton de evaluar
            //if (Hdf_Habilitar_Evaluar.Value == "false")
            //{
            //    Btn_Evaluar.Visible = false;
            //}
            //else
            //{
            //    Btn_Evaluar.Visible = true;
            //}
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }

    ///********************************************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Grid_Bandeja_Entrada_RowDataBound
    ///DESCRIPCIÓN          : cargara los botones
    ///PROPIEDADES          :
    ///CREO                 : Hugo Enrique Ramirez Aguilera
    /// FECHA_CREO          : 08/Mayo/2012
    ///MODIFICO             :
    ///FECHA_MODIFICO       :
    ///CAUSA_MODIFICACIÓN...:
    ///*********************************************************************************************************
    protected void Grid_Bandeja_Entrada_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        Cls_Cat_Tramites_Negocio Negocio_Tramite = new Cls_Cat_Tramites_Negocio();
        Cls_Ope_Bandeja_Tramites_Negocio Negocio_Tiempo_Estimado = new Cls_Ope_Bandeja_Tramites_Negocio();
        Cls_Ope_Bandeja_Tramites_Negocio Negocio_Tiempo_Final_Tramite = new Cls_Ope_Bandeja_Tramites_Negocio();
        var Dias_Inhabilies = new Cls_Ope_Pre_Dias_Inhabiles_Negocio();
        DateTime Fecha_Actual = DateTime.Today;
        DataTable Dt_Tiempo_Estimado;
        TimeSpan Diferencia_Dias;
        int Dias = 0;
        System.Drawing.Color Color_Grid = System.Drawing.Color.LightBlue;
        String Complemento = "";
        try
        {
            System.Web.UI.WebControls.Image Boton_Fecha = (System.Web.UI.WebControls.Image)e.Row.FindControl("Btn_Solicitud");

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Negocio_Tiempo_Estimado.P_Solicitud_ID = e.Row.Cells[1].Text;
                Dt_Tiempo_Estimado = Negocio_Tiempo_Estimado.Consultar_Tiempo_Estimado();
                Negocio_Tiempo_Final_Tramite = Negocio_Tiempo_Estimado.Consultar_Solicitud();

                e.Row.Cells[9].Visible = true;
                Complemento = e.Row.Cells[9].Text;
                e.Row.Cells[9].Visible = false ;

                //  para calcular el semaforo
                if (Negocio_Tiempo_Estimado.P_Solicitud_ID != "")
                {
                    Diferencia_Dias = (Negocio_Tiempo_Final_Tramite.P_Fecha_Entraga - Fecha_Actual);
                    Dias = Diferencia_Dias.Days;

                    Boton_Fecha.ToolTip = "Fecha_Entrega: " + String.Format("{0:dd/MMM/yyyy}", Negocio_Tiempo_Final_Tramite.P_Fecha_Entraga);
                    
                    if (Dias > 0)
                        Boton_Fecha.ImageUrl = "~/paginas/imagenes/gridview/circle_green.png";
                    
                    else if (Dias == 0)
                        Boton_Fecha.ImageUrl = "~/paginas/imagenes/gridview/circle_yellow.png";
                    
                    else
                        Boton_Fecha.ImageUrl = "~/paginas/imagenes/gridview/circle_red.png";
                }

                if (!String.IsNullOrEmpty(Complemento) && Complemento != "&nbsp;")
                {
                    for (int Cnt_Color_Grid = 0; Cnt_Color_Grid < 9; Cnt_Color_Grid++)
                    {
                        e.Row.Cells[Cnt_Color_Grid].BackColor = Color_Grid;
                    }
                }
                
            }
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Plantillas_RowDataBound
    ///DESCRIPCIÓN: Evento de RowDataBound del Grid de Plantillas.
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 16/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///******************************************************************************* 
    protected void Grid_Plantillas_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton Boton = (ImageButton)e.Row.Cells[4].FindControl("Btn_Generar_Documento");
                Boton.CommandArgument = e.Row.Cells[1].Text;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }
    
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Div_Grid_Datos_Tramite_RowDataBound
    ///DESCRIPCIÓN: Evento de RowDataBound del Grid de datos.
    ///PARAMETROS:     
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  28/Septiembre/2012 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///******************************************************************************* 
    protected void Div_Grid_Datos_Tramite_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                System.Web.UI.WebControls.TextBox Txt_Datos = (System.Web.UI.WebControls.TextBox)e.Row.Cells[3].FindControl("Txt_Valor_Dato");
                Txt_Datos.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }
     
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Datos_Dictamen_Modificar_RowDataBound
    ///DESCRIPCIÓN: Evento de RowDataBound del Grid de datos.
    ///PARAMETROS:     
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  28/Septiembre/2012 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///******************************************************************************* 
    protected void Grid_Datos_Dictamen_Modificar_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        Cls_Ope_Bandeja_Tramites_Negocio Negocio_Consultar_Tipo_Actividad = new Cls_Ope_Bandeja_Tramites_Negocio();

        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Negocio_Consultar_Tipo_Actividad.P_Solicitud_ID = Hdf_Solicitud_ID.Value;
                Negocio_Consultar_Tipo_Actividad = Negocio_Consultar_Tipo_Actividad.Consultar_Datos_Solicitud();

                System.Web.UI.WebControls.TextBox Txt_Datos_Dictamen = (System.Web.UI.WebControls.TextBox)e.Row.Cells[3].FindControl("Txt_Valor_Dato");

                if (Negocio_Consultar_Tipo_Actividad.P_Tipo_Actividad == "ELABORAR" || Negocio_Consultar_Tipo_Actividad.P_Complemento != "")
                    Txt_Datos_Dictamen.Enabled = true;

                else
                    Txt_Datos_Dictamen.Enabled = false;

            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }
    
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Datos_Dictamen_RowDataBound
    ///DESCRIPCIÓN: Evento de RowDataBound del Grid de datos.
    ///PARAMETROS:     
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  28/Septiembre/2012 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///******************************************************************************* 
    protected void Grid_Datos_Dictamen_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        Cls_Ope_Bandeja_Tramites_Negocio Negocio_Consultar_Tipo_Actividad = new Cls_Ope_Bandeja_Tramites_Negocio();

        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Negocio_Consultar_Tipo_Actividad.P_Solicitud_ID = Hdf_Solicitud_ID.Value;
                Negocio_Consultar_Tipo_Actividad = Negocio_Consultar_Tipo_Actividad.Consultar_Datos_Solicitud();

                System.Web.UI.WebControls.TextBox Txt_Datos_Dictamen = (System.Web.UI.WebControls.TextBox)e.Row.Cells[1].FindControl("Txt_Descripcion_Datos");

                if (Negocio_Consultar_Tipo_Actividad.P_Tipo_Actividad == "ELABORAR")
                    Txt_Datos_Dictamen.Enabled = true;

                else
                    Txt_Datos_Dictamen.Enabled = false;

            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Detalles_Formatos_RowDataBound
    ///DESCRIPCIÓN: Evento de RowDataBound del Grid de formatos.
    ///PARAMETROS:     
    ///CREO: Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO: 28/Mayo/2012 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///******************************************************************************* 
    protected void Grid_Detalles_Formatos_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        String Solicitud_ID = Hdf_Solicitud_ID.Value;
        String Nombre_Formato = "";
        Cls_Cat_Ort_Formato_Ficha_Inspeccion_Negocio Negocio_Formato_Ficha_Inspeccion = new Cls_Cat_Ort_Formato_Ficha_Inspeccion_Negocio();
        DataTable Dt_Consulta_Formato = new DataTable();
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Nombre_Formato = e.Row.Cells[2].Text.ToUpper();
                System.Web.UI.WebControls.CheckBox Chk_Confirma_Formato = (System.Web.UI.WebControls.CheckBox)e.Row.Cells[0].FindControl("Chk_Realizado");

                if (Nombre_Formato.Contains("ADMINISTRACION") || Nombre_Formato.Contains("CEDULA"))
                {
                    Negocio_Formato_Ficha_Inspeccion.P_Tabla = Ope_Ort_Formato_Admon_Urbana.Tabla_Ope_Ort_Formato_Admon_Urbana;
                    Negocio_Formato_Ficha_Inspeccion.P_Solicitud_ID = Solicitud_ID;
                    Negocio_Formato_Ficha_Inspeccion.P_Subproceso_ID = Hdf_Subproceso_ID.Value;
                    Dt_Consulta_Formato = Negocio_Formato_Ficha_Inspeccion.Consultar_Formato_Existente();

                    if (Dt_Consulta_Formato != null && Dt_Consulta_Formato.Rows.Count > 0)
                        Chk_Confirma_Formato.Checked = true;
                   
                }
                else if (Nombre_Formato.Contains("INSPECCION"))
                {
                    Negocio_Formato_Ficha_Inspeccion.P_Tabla = Ope_Ort_Formato_Ficha_Inspec.Tabla_Ope_Ort_Formato_Ficha_Inspec;
                    Negocio_Formato_Ficha_Inspeccion.P_Solicitud_ID = Solicitud_ID;
                    Negocio_Formato_Ficha_Inspeccion.P_Subproceso_ID = Hdf_Subproceso_ID.Value;
                    Dt_Consulta_Formato = Negocio_Formato_Ficha_Inspeccion.Consultar_Formato_Existente();

                    if (Dt_Consulta_Formato != null && Dt_Consulta_Formato.Rows.Count > 0)
                        Chk_Confirma_Formato.Checked = true;
                    
                }

            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Documentos_Tramite_RowDataBound
    ///DESCRIPCIÓN: Evento de RowDataBound del Grid de Documentos Anexados.
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 19/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///******************************************************************************* 
    protected void Grid_Documentos_Tramite_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton Boton = (ImageButton)e.Row.Cells[4].FindControl("Btn_Ver_Documento");
                Boton.CommandArgument = e.Row.Cells[0].Text;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Documentos_Seguimiento_RowDataBound
    ///DESCRIPCIÓN: Evento de RowDataBound del Grid de Documentos creados durante el 
    ///             seguimiento.
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 20/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///******************************************************************************* 
    protected void Grid_Documentos_Seguimiento_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton Boton = (ImageButton)e.Row.Cells[2].FindControl("Btn_Ver_Documento_Seguimiento");
                Boton.CommandArgument = e.Row.Cells[0].Text;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }

    #endregion

    #region Eventos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Solicitudes_Estatus
    ///DESCRIPCIÓN: Maneja la Busqueda de las Solicitudes por Estatus.
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 16/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///******************************************************************************* 
    protected void Btn_Buscar_Solicitudes_Estatus_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Consultar_Solicitudes_Tramites();
            Ocultar_Div(false);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }
    
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Reporte_Orden_Pago_Click
    ///DESCRIPCIÓN  : Genera el reporte de la orden de pago
    ///PARAMETROS   :     
    ///CREO         : Hugo Enrique Ramirez Aguilera
    ///FECHA_CREO   : 12/Noviembre/2012 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///******************************************************************************* 
    protected void Btn_Reporte_Orden_Pago_Click(object sender, ImageClickEventArgs e)
    {
        Cls_Ope_Bandeja_Tramites_Negocio Negocio_Datos_Solcitud = new Cls_Ope_Bandeja_Tramites_Negocio();
        try
        {
            Negocio_Datos_Solcitud.P_Solicitud_ID = HDN_Solicitud_ID.Value;
            Negocio_Datos_Solcitud = Negocio_Datos_Solcitud.Consultar_Datos_Solicitud();

            Imprimir_Reporte("PDF", Negocio_Datos_Solcitud, "INTERNO");
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }
    
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Llenar_Formato_Click
    ///DESCRIPCIÓN:Boton que enlazara con el llenado de la cedula
    ///PARAMETROS:     
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  20/Septiembre/20102
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///******************************************************************************* 
    protected void Btn_Llenar_Formato_Click(object sender, EventArgs e)
    {
        Session["Solicitud_Id"] = Hdf_Solicitud_ID.Value;
        FormsAuthentication.Initialize();
        Response.Redirect("../Ordenamiento_Territorial/Frm_Ope_Ort_Administracion_Urbana.aspx");
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Link_Cedula_Visita_Click
    ///DESCRIPCIÓN: habilita el div con los filtros para realizar la busqueda de los tramites
    ///PARAMETROS: 
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  21/Septiembre/20102
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Link_Cedula_Visita_Click(object sender, EventArgs e)
    {
        Session["Cedula_Solicitud_Id"] = Hdf_Solicitud_ID.Value;
        FormsAuthentication.Initialize();
        Response.Redirect("../Ordenamiento_Territorial/Frm_Ope_Ort_Administracion_Urbana.aspx");
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Link_Opiniones_Click
    ///DESCRIPCIÓN: habilita el div con los filtros para realizar la busqueda de los tramites
    ///PARAMETROS: 
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  26/Septiembre/20102
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Link_Opiniones_Click(object sender, EventArgs e)
    {
        Session["Opinion_Solicitud_Id"] = Hdf_Solicitud_ID.Value;
        FormsAuthentication.Initialize();
        Response.Redirect("../Ordenamiento_Territorial/Frm_Ope_Ort_Ficha_Revision.aspx");
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Cancelar_Solicitud_Click
    ///DESCRIPCIÓN: mostrara el grid de la bandeja de tramites
    ///PARAMETROS:     
    ///CREO:        Hugo Enrique Ramirez Aguilera
    ///FECHA_CREO:  23/Agosto/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///******************************************************************************* 
    protected void Btn_Cancelar_Solicitud_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Ocultar_Div(false);
            Mostrar_Filtro_Fecha();
            Consultar_Solicitudes_Tramites();
            Btn_Reporte_Orden_Pago.Visible = false;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }
    
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Generar_Documento_Click
    ///DESCRIPCIÓN: Maneja el Evento Click del Boton dentro de la tabla usado para ver 
    ///             la plantilla.
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 18/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///******************************************************************************* 
    protected void Btn_Generar_Documento_Click(object sender, ImageClickEventArgs e)
    {
        Cls_Ope_Bandeja_Tramites_Negocio Negocio_Solicitud = new Cls_Ope_Bandeja_Tramites_Negocio();
        Cls_Rpt_Ven_Consultar_Tramites_Negocio Negocio_Tramite = new Cls_Rpt_Ven_Consultar_Tramites_Negocio();
        Cls_Cat_Dependencias_Negocio Negocio_Dependencia = new Cls_Cat_Dependencias_Negocio();
        Cls_Ope_Solicitud_Tramites_Negocio Negocio_Actividades_Realizadas = new Cls_Ope_Solicitud_Tramites_Negocio();
        Cls_Cat_Ort_Parametros_Negocio Obj_Parametros = new Cls_Cat_Ort_Parametros_Negocio();
        Cls_Ope_Bandeja_Tramites_Negocio Solicitud = new Cls_Ope_Bandeja_Tramites_Negocio();

        DataTable Dt_Consulta = new DataTable();
        DataTable Dt_Ubicacion_Obra = new DataTable();

        String Dependencia_ID_Ordenamiento = "";
        String Dependencia_ID_Ambiental = "";
        String Dependencia_ID_Urbanistico = "";
        String Dependencia_ID_Inmobiliario = "";
        String Rol_Director_Ordenamiento = "";
        String Director_Ambiental = "";
        String Director_Urbanistico = "";
        String Director_Fraccionamientos = "";
        String Plantilla_Seleccionada = ""; 
        String Archivo = "";

        ImageButton Boton;
        try
        {

            // consultar parámetros
            Obj_Parametros.Consultar_Parametros();

            // dependencias
            if (!string.IsNullOrEmpty(Obj_Parametros.P_Dependencia_ID_Ordenamiento))
                Dependencia_ID_Ordenamiento = Obj_Parametros.P_Dependencia_ID_Ordenamiento;

            if (!string.IsNullOrEmpty(Obj_Parametros.P_Dependencia_ID_Ambiental))
                Dependencia_ID_Ambiental = Obj_Parametros.P_Dependencia_ID_Ambiental;

            if (!string.IsNullOrEmpty(Obj_Parametros.P_Dependencia_ID_Urbanistico))
                Dependencia_ID_Urbanistico = Obj_Parametros.P_Dependencia_ID_Urbanistico;

            if (!string.IsNullOrEmpty(Obj_Parametros.P_Dependencia_ID_Inmobiliario))
                Dependencia_ID_Inmobiliario = Obj_Parametros.P_Dependencia_ID_Inmobiliario;

            //  roles
            if (!string.IsNullOrEmpty(Obj_Parametros.P_Rol_Director_Ordenamiento))
                Rol_Director_Ordenamiento = Obj_Parametros.P_Rol_Director_Ordenamiento;

            if (!string.IsNullOrEmpty(Obj_Parametros.P_Rol_Director_Ambiental))
                Director_Ambiental = Obj_Parametros.P_Rol_Director_Ambiental;

            if (!string.IsNullOrEmpty(Obj_Parametros.P_Rol_Director_Fraccionamientos))
                Director_Fraccionamientos = Obj_Parametros.P_Rol_Director_Fraccionamientos;

            if (!string.IsNullOrEmpty(Obj_Parametros.P_Rol_Director_Urbana))
                Director_Urbanistico = Obj_Parametros.P_Rol_Director_Urbana;
            
            Negocio_Solicitud.P_Solicitud_ID = Hdf_Solicitud_ID.Value;
            Negocio_Solicitud = Negocio_Solicitud.Consultar_Datos_Solicitud();

            if (Cmb_Evaluacion.SelectedItem.Value.Equals("APROBAR"))
            {
                Boton = (ImageButton)sender;
                Plantilla_Seleccionada = Boton.CommandArgument;
                Hdf_Plantilla_Seleccionada.Value = Plantilla_Seleccionada;

                //  se busca la plantilla que se llenara
                for (Int32 Contador = 0; Contador < Grid_Plantillas.Rows.Count; Contador++)
                {
                    if (Plantilla_Seleccionada.Equals(Grid_Plantillas.Rows[Contador].Cells[1].Text.Trim()))
                    {
                        Archivo = HttpUtility.HtmlDecode(Grid_Plantillas.Rows[Contador].Cells[3].Text.Trim());
                        break;
                    }
                }

                //if (Archivo.ToUpper().Trim().Contains("DICTAMEN"))
                //{
                if ((!string.IsNullOrEmpty(Negocio_Solicitud.P_Dependencia_ID) && Negocio_Solicitud.P_Dependencia_ID == Dependencia_ID_Ordenamiento)
                    || (!string.IsNullOrEmpty(Negocio_Solicitud.P_Dependencia_ID) && Negocio_Solicitud.P_Dependencia_ID == Dependencia_ID_Ambiental)
                    || (!string.IsNullOrEmpty(Negocio_Solicitud.P_Dependencia_ID) && Negocio_Solicitud.P_Dependencia_ID == Dependencia_ID_Inmobiliario)
                    || (!string.IsNullOrEmpty(Negocio_Solicitud.P_Dependencia_ID) && Negocio_Solicitud.P_Dependencia_ID == Dependencia_ID_Urbanistico))
                {
                    if (Dependencia_ID_Inmobiliario == Negocio_Solicitud.P_Dependencia_ID)
                    {
                        #region Dependencia_ID_Inmobiliario

                        Boton = (ImageButton)sender;
                        Plantilla_Seleccionada = Boton.CommandArgument;
                        Hdf_Plantilla_Seleccionada.Value = Plantilla_Seleccionada;

                        //  se busca la plantilla que se llenara
                        for (Int32 Contador = 0; Contador < Grid_Plantillas.Rows.Count; Contador++)
                        {
                            if (Plantilla_Seleccionada.Equals(Grid_Plantillas.Rows[Contador].Cells[1].Text.Trim()))
                            {
                                Archivo = HttpUtility.HtmlDecode(Grid_Plantillas.Rows[Contador].Cells[3].Text.Trim());
                                break;
                            }
                        }

                        Negocio_Dependencia.P_Dependencia_ID = Negocio_Solicitud.P_Dependencia_ID;
                        DataTable Dt_Dependencia = Negocio_Dependencia.Consulta_Dependencias();


                        Negocio_Tramite.P_Solicitud_id = Hdf_Solicitud_ID.Value;
                        Dt_Consulta = Negocio_Tramite.Consultar_Tramites();


                        //  para la ubicacion de la obra
                        Negocio_Actividades_Realizadas.P_Cuenta_Predial = Txt_Cuenta_Predial.Text;
                        Dt_Ubicacion_Obra = Negocio_Actividades_Realizadas.Consultar_Datos_Obra();

                        //  se busca la plantilla que se llenara
                        for (Int32 Contador = 0; Contador < Grid_Plantillas.Rows.Count; Contador++)
                        {
                            if (Plantilla_Seleccionada.Equals(Grid_Plantillas.Rows[Contador].Cells[1].Text.Trim()))
                            {
                                Archivo =HttpUtility.HtmlDecode( Grid_Plantillas.Rows[Contador].Cells[3].Text.Trim());
                                break;
                            }
                        }

                        //  para obtener el nombre del archivo
                        String Nombre_Archivo = Negocio_Solicitud.P_Solicito + String.Format(" {0:yyyy-MM-dd   hh.mm.ss tt}", DateTime.Now);
                        Nombre_Archivo += "doc";
                        String Datos = "";

                        Solicitud.P_Solicitud_ID = Hdf_Solicitud_ID.Value;
                        Solicitud = Solicitud.Consultar_Datos_Solicitud();
                        DataTable Dt_Consulta_Dato_Final_Llenado = Solicitud.Consultar_Datos_Finales_Operacion();

                        if (Dt_Dependencia != null && Dt_Dependencia.Rows.Count > 0)
                            Datos += "<AREA>" + Dt_Dependencia.Rows[0][Cat_Dependencias.Campo_Nombre].ToString() + "</AREA>";

                        Datos += "<SOLICITUD_ID>" + Negocio_Solicitud.P_Folio + "</SOLICITUD_ID>";
                        Datos += "<Folio>" + Negocio_Solicitud.P_Folio + "</Folio>";
                        Datos += "<Folio_2>" + Negocio_Solicitud.P_Folio + "</Folio_2>";
                        Datos += "<Firma>" + Cmb_Supervisor_Zona.SelectedItem.ToString() + "</Firma>";

                        Datos += "<FECHA_DOCUMENTO>" + String.Format("{0:dd/MMM/yyyy}", DateTime.Today) + "</FECHA_DOCUMENTO>";

                        if (Dt_Ubicacion_Obra != null && Dt_Ubicacion_Obra.Rows.Count > 0)
                        {
                            Datos += "<Direccion_Solicitante>" + Dt_Ubicacion_Obra.Rows[0]["CALLE"].ToString() +
                                     " " + Dt_Ubicacion_Obra.Rows[0]["NUMERO"].ToString() +
                                     ", " + Dt_Ubicacion_Obra.Rows[0]["COLONIA"].ToString() + "</Direccion_Solicitante>";
                        }
                        else
                        {
                            Datos += "<Direccion_Solicitante>" + Solicitud.P_Calle_Predio +
                                                                 " " + Solicitud.P_Nuemro_Predio +
                                                                 ", " + Solicitud.P_Direccion_Predio + "</Direccion_Solicitante>";
                        }

                        //  para los datos a dictaminar
                        String Valor_Dictamen = "";
                        if (Dt_Consulta_Dato_Final_Llenado != null && Dt_Consulta_Dato_Final_Llenado.Rows.Count > 0)
                        {
                            int Contador = 0;
                            foreach (DataRow Registro in Dt_Consulta_Dato_Final_Llenado.Rows)
                            {
                                if (Contador > 0)
                                {
                                    Valor_Dictamen += ", ";
                                }
                                Valor_Dictamen += Registro["NOMBRE_DATO"].ToString();
                                Valor_Dictamen += " " + Registro["VALOR"].ToString();
                                Contador++;
                            }
                        }
                        Datos += "<Metros>" + Valor_Dictamen + "</Metros>";

                        Datos += "<Nombre_Solicitante>" + Txt_Propietario_Cuenta_Predial.Text + "</Nombre_Solicitante>";
                        Datos += "<CIUDAD_RESOLUCION> Irapuato </CIUDAD_RESOLUCION>";
                        Datos += "<ESTADO_RESOLUCION> Guanajuato </ESTADO_RESOLUCION>";

                        Datos += "<FECHA_SOLICITUD> " + String.Format("{0:dd/MMM/yyyy}", Negocio_Solicitud.P_Fecha_Solicitud) + " </FECHA_SOLICITUD>";

                        Datos += "<SOLICIUTD_ID_ENCABEZADO> " + Negocio_Solicitud.P_Folio + " </SOLICIUTD_ID_ENCABEZADO>";

                        MainDocumentPart main;
                        CustomXmlPart CustomXml;
                        StringBuilder newXml = new StringBuilder();
                        String Ruta_Plantilla = MapPath("../../Plantillas_Word/" + Archivo);
                        String Documento_Salida = Server.MapPath("../../Archivos/" + Negocio_Solicitud.P_Clave_Solicitud.Trim() + "/" + Nombre_Archivo); ;
                        String Carpeta_Principal = Server.MapPath("../../Archivos/" + Negocio_Solicitud.P_Clave_Solicitud.Trim());

                        //eliminamos el documento si es que existe
                        if (System.IO.Directory.Exists(Server.MapPath("../../Archivos")))
                        {
                            if (System.IO.File.Exists(Documento_Salida))
                            {
                                System.IO.File.Delete(Documento_Salida);
                            }
                        }
                        else
                        {
                            System.IO.Directory.CreateDirectory(Server.MapPath("../../Archivos"));
                        }

                        //verifica si existe un directorio llamado con ese Nombre_Commando de expediente
                        if (!Directory.Exists(Carpeta_Principal))
                        {
                            Directory.CreateDirectory(Carpeta_Principal);
                        }//fin if si existe directorio expediente

                        //copiamos la plantilla
                        File.Copy(Ruta_Plantilla, Documento_Salida);

                        using (WordprocessingDocument doc = WordprocessingDocument.Open(Documento_Salida, true))
                        {
                            newXml.Append("<root>");
                            newXml.Append(Datos);
                            newXml.Append("</root>");

                            main = doc.MainDocumentPart;
                            main.DeleteParts<CustomXmlPart>(main.CustomXmlParts);
                            CustomXml = main.AddCustomXmlPart(CustomXmlPartType.CustomXml);

                            using (StreamWriter ts = new StreamWriter(CustomXml.GetStream()))
                            {
                                ts.Write(newXml);
                            }
                            // guardar los cambios en el documento
                            main.Document.Save();

                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Btn_Generar_Documento_Click", "alert('Documento Creado');", true);

                            for (Int32 Contador = 0; Contador < Grid_Plantillas.Rows.Count; Contador++)
                            {
                                if (Grid_Plantillas.Rows[Contador].Cells[1].Text.Equals(Hdf_Plantilla_Seleccionada.Value))
                                {
                                    System.Web.UI.WebControls.CheckBox Check_Temporal = (System.Web.UI.WebControls.CheckBox)Grid_Plantillas.Rows[Contador].FindControl("Chk_Realizado");
                                    Check_Temporal.Checked = true;
                                    break;
                                }
                            }

                            //  se carga el historial de los archivos creados
                            Cargar_Documentos_Seguimiento(Negocio_Solicitud, Rol_Director_Ordenamiento, Director_Ambiental, Director_Fraccionamientos, Director_Urbanistico);
                        }
                    }

                    #endregion

                    //  para la area de urbanistico
                    else if (Dependencia_ID_Urbanistico == Negocio_Solicitud.P_Dependencia_ID)
                    {
                        #region Dependencia_ID_Urbanistico
                        Boton = (ImageButton)sender;
                        Plantilla_Seleccionada = Boton.CommandArgument;
                        Hdf_Plantilla_Seleccionada.Value = Plantilla_Seleccionada;


                        Negocio_Dependencia.P_Dependencia_ID = Negocio_Solicitud.P_Dependencia_ID;
                        DataTable Dt_Dependencia = Negocio_Dependencia.Consulta_Dependencias();


                        Negocio_Tramite.P_Solicitud_id = Hdf_Solicitud_ID.Value;
                        Dt_Consulta = Negocio_Tramite.Consultar_Tramites();


                        //  para la ubicacion de la obra
                        Negocio_Actividades_Realizadas.P_Cuenta_Predial = Txt_Cuenta_Predial.Text;
                        Dt_Ubicacion_Obra = Negocio_Actividades_Realizadas.Consultar_Datos_Obra();

                        //  se busca la plantilla que se llenara
                        for (Int32 Contador = 0; Contador < Grid_Plantillas.Rows.Count; Contador++)
                        {
                            if (Plantilla_Seleccionada.Equals(Grid_Plantillas.Rows[Contador].Cells[1].Text.Trim()))
                            {
                                Archivo = HttpUtility.HtmlDecode(Grid_Plantillas.Rows[Contador].Cells[3].Text.Trim());
                                break;
                            }
                        }

                        //  para obtener el nombre del archivo
                        String Nombre_Archivo = Negocio_Solicitud.P_Solicito + String.Format(" {0:yyyy-MM-dd   hh.mm.ss tt}", DateTime.Now);
                        Nombre_Archivo += "doc";
                        String Datos = "";

                        Solicitud.P_Solicitud_ID = Hdf_Solicitud_ID.Value;
                        Solicitud = Solicitud.Consultar_Datos_Solicitud();
                        DataTable Dt_Consulta_Dato_Final_Llenado = Solicitud.Consultar_Datos_Finales_Operacion();

                        if (Dt_Dependencia != null && Dt_Dependencia.Rows.Count > 0)
                            Datos += "<AREA>" + Dt_Dependencia.Rows[0][Cat_Dependencias.Campo_Nombre].ToString() + "</AREA>";

                        Datos += "<SOLICITUD_ID>" + Negocio_Solicitud.P_Folio + "</SOLICITUD_ID>";
                        Datos += "<Firma>" + Cmb_Supervisor_Zona.SelectedItem.ToString() + "</Firma>";

                        Datos += "<FECHA_DOCUMENTO>" + String.Format("{0:dd/MMM/yyyy}", DateTime.Today) + "</FECHA_DOCUMENTO>";

                        if (Dt_Ubicacion_Obra != null && Dt_Ubicacion_Obra.Rows.Count > 0)
                        {
                            Datos += "<Direccion_Solicitante>" + Dt_Ubicacion_Obra.Rows[0]["CALLE"].ToString() +
                                     " " + Dt_Ubicacion_Obra.Rows[0]["NUMERO"].ToString() +
                                     ", " + Dt_Ubicacion_Obra.Rows[0]["COLONIA"].ToString() + "</Direccion_Solicitante>";
                        }
                        else
                        {
                            Datos += "<Direccion_Solicitante>" + Solicitud.P_Calle_Predio +
                                                                 " " + Solicitud.P_Nuemro_Predio +
                                                                 ", " + Solicitud.P_Direccion_Predio + "</Direccion_Solicitante>";
                        }

                        //  para los datos a dictaminar
                        String Valor_Dictamen = "";
                        if (Dt_Consulta_Dato_Final_Llenado != null && Dt_Consulta_Dato_Final_Llenado.Rows.Count > 0)
                        {
                            int Contador = 0;
                            foreach (DataRow Registro in Dt_Consulta_Dato_Final_Llenado.Rows)
                            {
                                if (Contador > 0)
                                {
                                    Valor_Dictamen += ", ";
                                }
                                Valor_Dictamen += Registro["NOMBRE_DATO"].ToString();
                                Valor_Dictamen += " " + Registro["VALOR"].ToString();
                                Contador++;
                            }
                        }
                        Datos += "<Metros>" + Valor_Dictamen + "</Metros>";
                        Datos += "<Nombre_Solicitante>" + Txt_Propietario_Cuenta_Predial.Text + "</Nombre_Solicitante>";
                        Datos += "<SOLICIUTD_ID_ENCABEZADO> " + Negocio_Solicitud.P_Folio + " </SOLICIUTD_ID_ENCABEZADO>";

                        MainDocumentPart main;
                        CustomXmlPart CustomXml;
                        StringBuilder newXml = new StringBuilder();
                        String Ruta_Plantilla = MapPath("../../Plantillas_Word/" + Archivo);
                        String Documento_Salida = Server.MapPath("../../Archivos/" + Negocio_Solicitud.P_Clave_Solicitud.Trim() + "/SUB_Actividad_" + Hdf_Subproceso_ID.Value.Trim() + "_Archivo_" + Nombre_Archivo); ;
                        String Carpeta_Principal = Server.MapPath("../../Archivos/" + Negocio_Solicitud.P_Clave_Solicitud.Trim());

                        //verifica si existe un directorio llamado con ese Nombre_Commando de expediente
                        if (!Directory.Exists(Carpeta_Principal))
                        {
                            Directory.CreateDirectory(Carpeta_Principal);
                        }//fin if si existe directorio expediente

                        //copiamos la plantilla
                        File.Copy(Ruta_Plantilla, Documento_Salida);

                        using (WordprocessingDocument doc = WordprocessingDocument.Open(Documento_Salida, true))
                        {
                            newXml.Append("<root>");
                            newXml.Append(Datos);
                            newXml.Append("</root>");

                            main = doc.MainDocumentPart;
                            main.DeleteParts<CustomXmlPart>(main.CustomXmlParts);
                            CustomXml = main.AddCustomXmlPart(CustomXmlPartType.CustomXml);

                            using (StreamWriter ts = new StreamWriter(CustomXml.GetStream()))
                            {
                                ts.Write(newXml);
                            }
                            // guardar los cambios en el documento
                            main.Document.Save();

                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Btn_Generar_Documento_Click", "alert('Documento Creado');", true);

                            for (Int32 Contador = 0; Contador < Grid_Plantillas.Rows.Count; Contador++)
                            {
                                if (Grid_Plantillas.Rows[Contador].Cells[1].Text.Equals(Hdf_Plantilla_Seleccionada.Value))
                                {
                                    System.Web.UI.WebControls.CheckBox Check_Temporal = (System.Web.UI.WebControls.CheckBox)Grid_Plantillas.Rows[Contador].FindControl("Chk_Realizado");
                                    Check_Temporal.Checked = true;
                                    break;
                                }
                            }

                            //  se carga el historial de los archivos creados
                            Cargar_Documentos_Seguimiento(Negocio_Solicitud, Rol_Director_Ordenamiento, Director_Ambiental, Director_Fraccionamientos, Director_Urbanistico);

                        }// fin del using
                        #endregion
                    }

                    //  Inicio Ambiental
                    else if (Dependencia_ID_Ambiental == Negocio_Solicitud.P_Dependencia_ID)
                    {
                        #region Dependencia_ID_Ambiental
                        Boton = (ImageButton)sender;
                        Plantilla_Seleccionada = Boton.CommandArgument;
                        Hdf_Plantilla_Seleccionada.Value = Plantilla_Seleccionada;

                        Negocio_Dependencia.P_Dependencia_ID = Negocio_Solicitud.P_Dependencia_ID;
                        DataTable Dt_Dependencia = Negocio_Dependencia.Consulta_Dependencias();

                        Negocio_Tramite.P_Solicitud_id = Hdf_Solicitud_ID.Value;
                        Dt_Consulta = Negocio_Tramite.Consultar_Tramites();

                        //  para la ubicacion de la obra
                        Negocio_Actividades_Realizadas.P_Cuenta_Predial = Txt_Cuenta_Predial.Text;
                        Dt_Ubicacion_Obra = Negocio_Actividades_Realizadas.Consultar_Datos_Obra();

                        //  se busca la plantilla que se llenara
                        for (Int32 Contador = 0; Contador < Grid_Plantillas.Rows.Count; Contador++)
                        {
                            if (Plantilla_Seleccionada.Equals(Grid_Plantillas.Rows[Contador].Cells[1].Text.Trim()))
                            {
                                Archivo = HttpUtility.HtmlDecode(Grid_Plantillas.Rows[Contador].Cells[3].Text.Trim());
                                break;
                            }
                        }

                        //  para obtener el nombre del archivo
                        String Nombre_Archivo = Negocio_Solicitud.P_Solicito + String.Format(" {0:yyyy-MM-dd   hh.mm.ss tt}", DateTime.Now);
                        Nombre_Archivo += "doc";
                        String Datos = "";

                        Solicitud.P_Solicitud_ID = Hdf_Solicitud_ID.Value;
                        Solicitud = Solicitud.Consultar_Datos_Solicitud();
                        DataTable Dt_Consulta_Dato_Final_Llenado = Solicitud.Consultar_Datos_Finales_Operacion();

                        Datos += "<FOLIO_ENCABEZADO>" + Negocio_Solicitud.P_Folio + "</FOLIO_ENCABEZADO>";
                        Datos += "<FECHA_DOCUMENTO>" + String.Format("{0:d}", DateTime.Now.Day) + " de " + String.Format("{0:MMMM}", DateTime.Now) + " del año " + String.Format("{0:yyyy}", DateTime.Now) + "</FECHA_DOCUMENTO>";
                        Datos += "<NOMBRE_SOLICITA>" + Negocio_Solicitud.P_Solicito + "</NOMBRE_SOLICITA>";


                        Cls_Ope_Pre_Resumen_Predio_Negocio Predio = new Cls_Ope_Pre_Resumen_Predio_Negocio();
                        Predio.P_Cuenta_Predial = Solicitud.P_Cuenta_Predial;
                        DataTable Dt_Cuenta = Predio.Consulta_Datos_Cuenta_Generales().Tables[0];

                        if (Dt_Cuenta != null && Dt_Cuenta.Rows.Count > 0)
                        {
                            Predio.P_Calle_ID = Dt_Cuenta.Rows[0]["CALLE_ID_NOTIFICACION"].ToString();
                            Predio.P_Colonia_ID = Dt_Cuenta.Rows[0]["COLONIA_ID_NOTIFICACION"].ToString();
                            Predio.P_Ciudad_ID = Dt_Cuenta.Rows[0]["CIUDAD_ID_NOTIFICACION"].ToString();
                            Predio.P_Estado_Predio = Dt_Cuenta.Rows[0]["ESTADO_ID_NOTIFICACION"].ToString();

                            DataTable Dt_Calle = Predio.Consultar_Calle_Generales();
                            if (Dt_Calle != null && Dt_Calle.Rows.Count > 0)
                                Datos += "<CALLE_NOTIFICACIONES>" + Dt_Calle.Rows[0][Cat_Pre_Calles.Campo_Nombre].ToString() + "</CALLE_NOTIFICACIONES>";

                            Datos += "<NO_NOTIFICACIONES>" + Dt_Cuenta.Rows[0]["NO_EXTERIOR_NOTIFICACION"].ToString() + "</NO_NOTIFICACIONES>";

                            DataTable Dt_Colonia = Predio.Consultar_Colonia_Generales();
                            if (Dt_Colonia != null && Dt_Colonia.Rows.Count > 0)
                                Datos += "<COLONIA_NOTIFICACIONES>" + Dt_Colonia.Rows[0][Cat_Ate_Colonias.Campo_Nombre].ToString() + "</COLONIA_NOTIFICACIONES>";

                            DataTable Dt_Ciudad = Predio.Consultar_Ciudad();
                            if (Dt_Ciudad != null && Dt_Ciudad.Rows.Count > 0)
                                Datos += "<MUNICIPIO_NOTIFICACIONES>" + Dt_Ciudad.Rows[0][Cat_Pre_Ciudades.Campo_Nombre].ToString() + "</MUNICIPIO_NOTIFICACIONES>";

                            DataTable Dt_Estado = Predio.Consultar_Estado_Predio_Propietario();
                            if (Dt_Estado != null && Dt_Estado.Rows.Count > 0)
                                Datos += "<ESTADO_NOTIFICACIONES>" + Dt_Estado.Rows[0]["Descripcion"].ToString() + "</ESTADO_NOTIFICACIONES>";

                            Predio.P_Calle_ID = Dt_Cuenta.Rows[0]["CALLE_ID1"].ToString();
                            Predio.P_Colonia_ID = Dt_Cuenta.Rows[0]["COLONIA_ID1"].ToString();

                            Dt_Calle = Predio.Consultar_Calle_Generales();
                            if (Dt_Calle != null && Dt_Calle.Rows.Count > 0)
                                Datos += "<CALLE_UBICACION>" + Dt_Calle.Rows[0][Cat_Pre_Calles.Campo_Nombre].ToString() + "</CALLE_UBICACION>";

                            Datos += "<NO_UBICACION>" + Dt_Cuenta.Rows[0]["NO_EXTERIOR"].ToString() + "</NO_UBICACION>";

                            Dt_Colonia = Predio.Consultar_Colonia_Generales();
                            if (Dt_Colonia != null && Dt_Colonia.Rows.Count > 0)
                                Datos += "<COLONIA_UBICACION>" + Dt_Colonia.Rows[0][Cat_Ate_Colonias.Campo_Nombre].ToString() + "</COLONIA_UBICACION>";
                        }

                        else
                        {
                            Datos += "<CALLE_NOTIFICACIONES>" + Solicitud.P_Calle_Predio + "</CALLE_NOTIFICACIONES>";
                            Datos += "<NO_NOTIFICACIONES>" + Solicitud.P_Nuemro_Predio + "</NO_NOTIFICACIONES>";
                            Datos += "<COLONIA_NOTIFICACIONES>" + Solicitud.P_Direccion_Predio + "</COLONIA_NOTIFICACIONES>";
                            Datos += "<MUNICIPIO_NOTIFICACIONES>Irapuato</MUNICIPIO_NOTIFICACIONES>";
                            Datos += "<ESTADO_NOTIFICACIONES>Guanajuato</ESTADO_NOTIFICACIONES>";
                            Datos += "<CALLE_UBICACION>" + Solicitud.P_Calle_Predio + "</CALLE_UBICACION>";
                            Datos += "<NO_UBICACION>" + Solicitud.P_Nuemro_Predio + "</NO_UBICACION>";
                            Datos += "<COLONIA_UBICACION>" + Solicitud.P_Direccion_Predio + "</COLONIA_UBICACION>";
                        }

                        Datos += "<FECHA_SOLICITUD>" + String.Format("dd/MMM/yyyy", Solicitud.P_Fecha_Solicitud) + "</FECHA_SOLICITUD>";

                        if (Dt_Consulta_Dato_Final_Llenado.Rows.Count > 0)
                        {
                            Datos += "<DATOS>";
                            foreach (DataRow Dr_Row in Dt_Consulta_Dato_Final_Llenado.Rows)
                            {
                                Datos += Dr_Row["VALOR"].ToString() + " para " + Dr_Row["NOMBRE_DATO"].ToString() + ", ";
                            }
                            Datos = Datos.Substring(0, Datos.Length - 1);
                            Datos += "</DATOS>";
                        }

                        Datos += "<FOLIO_RESUELVE>" + Negocio_Solicitud.P_Folio + "</FOLIO_RESUELVE>";

                        if (Dt_Consulta_Dato_Final_Llenado.Rows.Count > 0)
                        {
                            Datos += "<DATOS_IMPACTOS>";
                            foreach (DataRow Dr_Row in Dt_Consulta_Dato_Final_Llenado.Rows)
                            {
                                Datos += Dr_Row["VALOR"].ToString() + " para " + Dr_Row["NOMBRE_DATO"].ToString() + ", ";
                            }
                            Datos = Datos.Substring(0, Datos.Length - 3);
                            Datos += ".</DATOS_IMPACTOS>";
                        }

                        if (Dt_Dependencia != null && Dt_Dependencia.Rows.Count > 0)
                            Datos += "<RESPONSABLE>" + Cmb_Supervisor_Zona.SelectedItem.Text.ToString() + "</RESPONSABLE>";

                        MainDocumentPart main;
                        CustomXmlPart CustomXml;
                        StringBuilder newXml = new StringBuilder();
                        String Ruta_Plantilla = MapPath("../../Plantillas_Word/" + Archivo);
                        String Documento_Salida = Server.MapPath("../../Archivos/" + Negocio_Solicitud.P_Clave_Solicitud.Trim() + "/"  + Nombre_Archivo); ;
                        String Carpeta_Principal = Server.MapPath("../../Archivos/" + Negocio_Solicitud.P_Clave_Solicitud.Trim());

                        //verifica si existe un directorio llamado con ese Nombre_Commando de expediente
                        if (!Directory.Exists(Carpeta_Principal))
                        {
                            Directory.CreateDirectory(Carpeta_Principal);
                        }//fin if si existe directorio expediente

                        //copiamos la plantilla
                        File.Copy(Ruta_Plantilla, Documento_Salida);

                        using (WordprocessingDocument doc = WordprocessingDocument.Open(Documento_Salida, true))
                        {
                            newXml.Append("<root>");
                            newXml.Append(Datos);
                            newXml.Append("</root>");

                            main = doc.MainDocumentPart;
                            main.DeleteParts<CustomXmlPart>(main.CustomXmlParts);
                            CustomXml = main.AddCustomXmlPart(CustomXmlPartType.CustomXml);

                            using (StreamWriter ts = new StreamWriter(CustomXml.GetStream()))
                            {
                                ts.Write(newXml);
                            }
                            // guardar los cambios en el documento
                            main.Document.Save();

                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Btn_Generar_Documento_Click", "alert('Documento Creado');", true);

                            for (Int32 Contador = 0; Contador < Grid_Plantillas.Rows.Count; Contador++)
                            {
                                if (Grid_Plantillas.Rows[Contador].Cells[1].Text.Equals(Hdf_Plantilla_Seleccionada.Value))
                                {
                                    System.Web.UI.WebControls.CheckBox Check_Temporal = (System.Web.UI.WebControls.CheckBox)Grid_Plantillas.Rows[Contador].FindControl("Chk_Realizado");
                                    Check_Temporal.Checked = true;
                                    break;
                                }
                            }

                            //  se carga el historial de los archivos creados
                            Cargar_Documentos_Seguimiento(Negocio_Solicitud, Rol_Director_Ordenamiento, Director_Ambiental, Director_Fraccionamientos, Director_Urbanistico);
                        }
                        #endregion
                    }
                }
                else
                {
                    if (sender != null)
                    {
                        Boton = (ImageButton)sender;
                        Plantilla_Seleccionada = Boton.CommandArgument;
                        Hdf_Plantilla_Seleccionada.Value = Plantilla_Seleccionada;

                        for (Int32 Contador = 0; Contador < Grid_Plantillas.Rows.Count; Contador++)
                        {
                            if (Plantilla_Seleccionada.Equals(Grid_Plantillas.Rows[Contador].Cells[1].Text.Trim()))
                            {
                                Archivo = HttpUtility.HtmlDecode(Grid_Plantillas.Rows[Contador].Cells[3].Text.Trim());
                                break;
                            }
                        }

                        String Ruta_Archivo = Server.MapPath("../../Plantillas_Word/" + Archivo);
                        Cls_Ayudante_Llenado_Plantilla_Word Ayudante_Plantilla = new Cls_Ayudante_Llenado_Plantilla_Word();
                        String Nombre_Archivo = Obtener_Extension(Archivo) + ".doc";
                        String Ruta_Plantilla = MapPath("../../Plantillas_Word/" + Archivo);
                        String Documento_Salida = Server.MapPath("../../Archivos/" + Negocio_Solicitud.P_Clave_Solicitud.Trim() + "/" + Hdf_Subproceso_ID.Value.Trim() + "_Archivo_" + Nombre_Archivo); ;
                        String Carpeta_Principal = Server.MapPath("../../Archivos/" + Negocio_Solicitud.P_Clave_Solicitud.Trim());

                        //  se obtienen los marcadores de la plantilla
                        List<Tag> Lista_Marcadores = Ayudante_Plantilla.Obtener_Etiquetas_Word(Ruta_Archivo);

                        //  se cargara la informacion de la base de datos
                        Solicitud.P_Solicitud_ID = Hdf_Solicitud_ID.Value;
                        Solicitud = Solicitud.Consultar_Datos_Solicitud();
                        Solicitud.P_Tipo_Dato = "INICIAL";
                        DataTable Dt_Consulta_Dato_Final_Llenado = Solicitud.Consultar_Datos_Finales_Operacion();

                        //  se carga la informacion en los marcadores
                        XmlDocument _Xml = Ayudante_Plantilla.Llenar_Etiquetas_Word(Lista_Marcadores, Dt_Consulta_Dato_Final_Llenado);

                        //  se genera el documento
                        Boolean Estado_Archivo = Ayudante_Plantilla.Generar_Documento_Word(Ruta_Plantilla, Documento_Salida, Carpeta_Principal, _Xml);

                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Btn_Generar_Documento_Click", "alert('Documento Creado');", true);

                        for (Int32 Contador = 0; Contador < Grid_Plantillas.Rows.Count; Contador++)
                        {
                            if (Grid_Plantillas.Rows[Contador].Cells[1].Text.Equals(Hdf_Plantilla_Seleccionada.Value))
                            {
                                System.Web.UI.WebControls.CheckBox Check_Temporal = (System.Web.UI.WebControls.CheckBox)Grid_Plantillas.Rows[Contador].FindControl("Chk_Realizado");
                                Check_Temporal.Checked = true;
                                break;
                            }
                        }

                        //  se carga el historial de los archivos creados
                        Cargar_Documentos_Seguimiento(Negocio_Solicitud, Rol_Director_Ordenamiento, Director_Ambiental, Director_Fraccionamientos, Director_Urbanistico);


                    }// fin del if (sender != null)
                }// Fin del else
            }// fin del if cmb_evaluar
            else
            {
                Lbl_Mensaje_Error.Text = "Para el estatus de '" + Cmb_Evaluacion.SelectedItem.Value + "':";
                Lbl_Mensaje_Error.Text += " + No es necesario llenar las Plantillas";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }
        catch (Exception ex)
        {
            Mensaje_Error(true,  ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Evaluar_Click
    ///DESCRIPCIÓN: Se maneja el evento del boton de evaluacion y envia los datos para 
    ///              ser actualizados.
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 18/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///******************************************************************************* 
    protected void Btn_Evaluar_Click(object sender, ImageClickEventArgs e)
    {
        Cls_Ope_Bandeja_Tramites_Negocio Solicitud = new Cls_Ope_Bandeja_Tramites_Negocio();
        Cls_Ope_Bandeja_Tramites_Negocio Negocio_Consultar_Dias = new Cls_Ope_Bandeja_Tramites_Negocio();
        Cls_Ope_Solicitud_Tramites_Negocio Solicitud_Negocio = new Cls_Ope_Solicitud_Tramites_Negocio();
        try
        {
            if (Validar_Evaluacion())
            {
                Lbl_Mensaje_Error.Visible = false;
                Div_Contenedor_Msj_Error.Visible = false;

                Solicitud.P_Solicitud_ID = Hdf_Solicitud_ID.Value;
                Negocio_Consultar_Dias.P_Solicitud_ID = Hdf_Solicitud_ID.Value;
                Negocio_Consultar_Dias = Negocio_Consultar_Dias.Consultar_Datos_Solicitud();

                if (Txt_Fecha_Vigencia_Inicio.Text != "")
                    Solicitud.P_Fecha_Vigencia_Inicio = Txt_Fecha_Vigencia_Inicio.Text;

                if (Txt_Fecha_Vigencia_Fin.Text != "")
                    Solicitud.P_Fecha_Vigencia_Fin = Txt_Fecha_Vigencia_Fin.Text;

                if (Txt_Fecha_Doc_Inicio.Text != "")
                    Solicitud.P_Fecha_Documento_Vigencia_inicio = Txt_Fecha_Doc_Inicio.Text;

                if (Txt_Fecha_Doc_Fin.Text != "")
                    Solicitud.P_Fecha_Documento_Vigencia_Fin = Txt_Fecha_Doc_Fin.Text;

                //  para la ubicacion del expediente
                if (Txt_Ubicacion_Expediente.Text != "")
                    Solicitud.P_Ubicacion_Expediente = Txt_Ubicacion_Expediente.Text;

                Solicitud.P_SubProceso_Anterior = Negocio_Consultar_Dias.P_SubProceso_Anterior;
                Solicitud.P_Estatus = Cmb_Evaluacion.SelectedItem.Value;
                Solicitud.P_Tramite_id = Negocio_Consultar_Dias.P_Tramite_id;

                Solicitud.P_Subproceso_ID = Hdf_Subproceso_ID.Value;
                Solicitud.P_Usuario = Cls_Sessiones.Nombre_Empleado;

                //  validacion para la convercion del costo
                if (!String.IsNullOrEmpty(Txt_Costo_Total.Text))
                    Solicitud.P_Costo_Total = Convert.ToDouble(Txt_Costo_Total.Text);

                //  campo comentarios
                Solicitud.P_Comentarios = Txt_Comentarios_Evaluacion.Text.Trim();
                Solicitud.P_Comentarios_Internos = Txt_Comentarios_Internos.Text;


                if (Cmb_Inspector.SelectedIndex > 0)
                    Solicitud.P_Persona_Inspecciona = Cmb_Inspector.SelectedValue.ToString();

                //  para la clave del tramite
                Cls_Ope_Solicitud_Tramites_Negocio Rs_Alta = new Cls_Ope_Solicitud_Tramites_Negocio();
                Rs_Alta.P_Tramite_ID = Hdf_Tramite_Id.Value;
                DataSet Ds_Tramite = Rs_Alta.Consultar_Tramites();
                if (Ds_Tramite.Tables.Count > 0)
                    if (Ds_Tramite.Tables[0].Rows.Count > 0)
                        Solicitud.P_Folio = Ds_Tramite.Tables[0].Rows[0][Cat_Tra_Tramites.Campo_Clave_Tramite].ToString();


                //  se cambiaran las fechas de la solicitud con el metodo de modificar para las solicitudes en ESTATUS PENDIENTE
                if (Negocio_Consultar_Dias.P_Estatus == "PENDIENTE")
                {
                    var Dias_Inhabilies = new Cls_Ope_Pre_Dias_Inhabiles_Negocio();
                    DateTime Fecha_Solucion = Dias_Inhabilies.Calcular_Fecha("" + DateTime.Today, "" + Negocio_Consultar_Dias.P_Tiempo_Estimado);
                    Solicitud.P_Fecha_Entraga = Fecha_Solucion;
                    Solicitud.Modificar_Fechas_Solicitud();
                }
                //  si existe una actividad condicion se cargan las variables p_condicion                
                if ((Hdf_Condicion_Si.Value != "0" || Hdf_Condicion_No.Value != "0") &&
                    (Hdf_Condicion_Si.Value != "" || Hdf_Condicion_No.Value != ""))
                {
                    Solicitud.P_Respuesta_Condicion = Cmb_Condicion.SelectedValue;
                    Solicitud.P_Condicion_Si = Convert.ToDouble(Hdf_Condicion_Si.Value);
                    Solicitud.P_Condicion_No = Convert.ToDouble(Hdf_Condicion_No.Value);
                }

                // si es una solicitud de ordenamiento territorial (campo zona visible)
                if (Pnl_Contenedor_Zona.Style.Value == "display:block")
                {
                    Solicitud.P_Empleado_ID = Cmb_Supervisor_Zona.SelectedValue;
                    Solicitud.P_Zona_ID = Cmb_Zonas.SelectedValue;
                }
                /**********************************  Evaluar solicitud *********************************************/
                Solicitud = Solicitud.Evaluar_Solicitud();
                /***************************************************************************************************/

                //  Se consulta la informacion de la solicitud
                Solicitud = Solicitud.Consultar_Datos_Solicitud();
                if (!String.IsNullOrEmpty(Solicitud.P_Email))
                {
                    if (Txt_Estatus.Text == "PENDIENTE" || Txt_Comentarios_Evaluacion.Text != "" || Solicitud.P_Tipo_Actividad == "COBRO")
                        Enviar_Correo_Notificacion(Solicitud, Txt_Estatus.Text, Txt_Subproceso.Text);
                }

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Bandeja", "alert('Evaluación Realizada Exitosamente');", true);
                Consultar_Solicitudes_Tramites();
                Tab_Contenedor_Pestagnas.ActiveTabIndex = 0;

                Limpiar_Formulario();
                Pnl_Contenedor_Zona.Style.Value = "display:none";
                Habilitar_Comentarios(false);
                AFU_Subir_Archivo.Enabled = false;

                Mostrar_Filtro_Fecha();
                Ocultar_Div(false);

                //  se limpian los grids
                Cargar_Grid_Actividades(new DataTable());
                Cargar_Grid_Comentarios(new DataTable());
                
                Btn_Reporte_Orden_Pago.Visible = false;
            }
            else
            {
                Lbl_Mensaje_Error.Visible = true;
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        catch (Exception ex)
        {
            throw new Exception(ex.Message);
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
        Cls_Ope_Bandeja_Tramites_Negocio Negocio_Modificacion = new Cls_Ope_Bandeja_Tramites_Negocio();
        try
        {
             if (Cmb_Zonas.SelectedIndex > 0)
            {
                Negocio_Modificacion.P_Zona_ID = Cmb_Zonas.SelectedValue;
                Negocio_Modificacion.P_Empleado_ID = Cmb_Supervisor_Zona.SelectedValue;
                Negocio_Modificacion.P_Solicitud_ID = Hdf_Solicitud_ID.Value;
                Negocio_Modificacion.Modificar_Zona();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Btn_Modificar_Click", "alert('Actualización de zona exitosa.');", true);
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }
    
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Guardar_Datos_Dictamen_Click
    ///DESCRIPCIÓN:guardara los datos finales del dictamen
    ///PROPIEDADES:     
    ///CREO: Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO: 20/Agosto/20102
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Guardar_Datos_Dictamen_Click(object sender, EventArgs e)
    {
        Cls_Ope_Bandeja_Tramites_Negocio Negocio_Modificacion = new Cls_Ope_Bandeja_Tramites_Negocio();
        DataTable Dt_Datos = (DataTable)(Session["Grid_Datos"]);
        DataTable Dt_Datos_Modificar = (DataTable)(Session["Grid_Datos_Modificar"]);
        String Actividad_ID = "";
        DataTable Dt_Siguiente_Actividad = new DataTable();
        try
        {
            if (Grid_Datos_Dictamen.Visible)
            {
                if (Validar_Datos_Grid_Datos())
                {
                    String[,] Datos = Obtener_Datos_Dictamen();
                    Negocio_Modificacion.P_Usuario = Cls_Sessiones.Nombre_Empleado;
                    Negocio_Modificacion.P_Datos = Datos; 
                    Negocio_Modificacion.P_Tramite_id= Hdf_Tramite_Id.Value; 
                    Negocio_Modificacion.P_Solicitud_ID = HDN_Solicitud_ID.Value;
                    Negocio_Modificacion.Alta_Datos_Dictamen();
                    Grid_Bandeja_Entrada_SelectedIndexChanged(sender, null);

                    if (!String.IsNullOrEmpty(Txt_Costo_Total.Text))
                    {
                        Negocio_Modificacion.P_Solicitud_ID = Hdf_Solicitud_ID.Value;
                        Negocio_Modificacion.P_Costo_Total =Convert.ToDouble(Txt_Costo_Total.Text);
                        Negocio_Modificacion.Modificar_Costo_Solicitud();
                    }
                }
            }
            else
            {
                if (Validar_Datos_Grid_Datos_Modificar())
                {
                    DataTable Dt_Datos_Mod = new DataTable();
                    Dt_Datos_Mod.Columns.Add("OPE_DATO_ID", typeof(String));
                    Dt_Datos_Mod.Columns.Add("VALOR", typeof(String));

                    foreach (GridViewRow Gr_Row in Grid_Datos_Dictamen_Modificar.Rows)
                    {
                        DataRow Fila = Dt_Datos_Mod.NewRow();
                        Fila["OPE_DATO_ID"] = Gr_Row.Cells[0].Text.ToString();
                        Fila["VALOR"] = ((TextBox)Gr_Row.Cells[3].FindControl("Txt_Valor_Dato")).Text;
                        Dt_Datos_Mod.Rows.Add(Fila);
                    }

                    Negocio_Modificacion.P_Usuario = Cls_Sessiones.Nombre_Empleado;
                    Negocio_Modificacion.P_Dt_Datos = Dt_Datos_Mod;
                    Negocio_Modificacion.Modificar_Datos_Dictamen();
                    Grid_Bandeja_Entrada_SelectedIndexChanged(sender, null);

                    if (!String.IsNullOrEmpty(Txt_Costo_Total.Text))
                    {
                        Negocio_Modificacion.P_Solicitud_ID = Hdf_Solicitud_ID.Value;
                        Negocio_Modificacion.P_Costo_Total = Convert.ToDouble(Txt_Costo_Total.Text);
                        Negocio_Modificacion.Modificar_Costo_Solicitud();
                    }
                }


                Negocio_Modificacion.P_Solicitud_ID = HDN_Solicitud_ID.Value;
                Negocio_Modificacion = Negocio_Modificacion.Consultar_Datos_Solicitud();
                Actividad_ID = "";
                Dt_Siguiente_Actividad = Negocio_Modificacion.Consultar_Siguiente_Actividad();

                if (Dt_Siguiente_Actividad != null && Dt_Siguiente_Actividad.Rows.Count > 0)
                {
                    Actividad_ID = Dt_Siguiente_Actividad.Rows[0][Cat_Tra_Subprocesos.Campo_Subproceso_ID].ToString();
                }

                Calcular_Costo(Actividad_ID, Negocio_Modificacion);
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }
    
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Actualizar_Documentos_Click
    ///DESCRIPCIÓN: carga el grid de documentos de seguimiento
    ///PROPIEDADES:     
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  14/Agosto/2012 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Actualizar_Documentos_Click(object sender, EventArgs e)
    {
        Cls_Cat_Ort_Parametros_Negocio Obj_Parametros = new Cls_Cat_Ort_Parametros_Negocio();
        Cls_Ope_Bandeja_Tramites_Negocio Negocio_Modificacion = new Cls_Ope_Bandeja_Tramites_Negocio();
        String Rol_Director_Ordenamiento = "";
        String Director_Ambiental = "";
        String Director_Urbanistico = "";
        String Director_Fraccionamientos = "";
        try
        {
            if (AFU_Subir_Archivo.HasFile)
            { 
                // consultar parámetros
                Obj_Parametros.Consultar_Parametros();

                // validar que la consulta haya regresado valor
                if (!string.IsNullOrEmpty(Obj_Parametros.P_Rol_Director_Ordenamiento))
                    Rol_Director_Ordenamiento = Obj_Parametros.P_Rol_Director_Ordenamiento;
                
                if (!string.IsNullOrEmpty(Obj_Parametros.P_Rol_Director_Ambiental))
                    Director_Ambiental = Obj_Parametros.P_Rol_Director_Ambiental;
               
                if (!string.IsNullOrEmpty(Obj_Parametros.P_Rol_Director_Fraccionamientos))
                    Director_Fraccionamientos = Obj_Parametros.P_Rol_Director_Fraccionamientos;
                
                if (!string.IsNullOrEmpty(Obj_Parametros.P_Rol_Director_Urbana))
                    Director_Urbanistico = Obj_Parametros.P_Rol_Director_Urbana;


                Negocio_Modificacion.P_Solicitud_ID = HDN_Solicitud_ID.Value;
                Negocio_Modificacion = Negocio_Modificacion.Consultar_Datos_Solicitud(); // Se obtienen los Datos a Detalle de la Solicitud Seleccionada
                Cargar_Documentos_Seguimiento(Negocio_Modificacion, Rol_Director_Ordenamiento, Director_Ambiental, Director_Fraccionamientos , Director_Urbanistico);
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Quitar_Documentos_Click
    ///DESCRIPCIÓN: quita el documento seleccionado del grid de documentos de seguimiento
    ///PROPIEDADES:     
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  14/Agosto/2012 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Quitar_Documentos_Click(object sender, EventArgs e)
    {
        Cls_Ope_Bandeja_Tramites_Negocio Negocio_Solicitud = new Cls_Ope_Bandeja_Tramites_Negocio();
        String Nombre_Archivo = "";
        Cls_Cat_Ort_Parametros_Negocio Obj_Parametros = new Cls_Cat_Ort_Parametros_Negocio();
        String Rol_Director_Ordenamiento = "";
        String Director_Ambiental = "";
        String Director_Urbanistico = "";
        String Director_Fraccionamientos = "";

        try
        {
            // consultar parámetros
            Obj_Parametros.Consultar_Parametros();

            // roles de los directores
            if (!string.IsNullOrEmpty(Obj_Parametros.P_Rol_Director_Ordenamiento))
                Rol_Director_Ordenamiento = Obj_Parametros.P_Rol_Director_Ordenamiento;
            
            if (!string.IsNullOrEmpty(Obj_Parametros.P_Rol_Director_Ambiental))
                Director_Ambiental = Obj_Parametros.P_Rol_Director_Ambiental;
            
            if (!string.IsNullOrEmpty(Obj_Parametros.P_Rol_Director_Fraccionamientos))
                Director_Fraccionamientos = Obj_Parametros.P_Rol_Director_Fraccionamientos;
            
            if (!string.IsNullOrEmpty(Obj_Parametros.P_Rol_Director_Urbana))
                Director_Urbanistico = Obj_Parametros.P_Rol_Director_Urbana;


            ImageButton ImageButton = (ImageButton)sender;
            System.Web.UI.WebControls.TableCell TableCell = (System.Web.UI.WebControls.TableCell)ImageButton.Parent;
            GridViewRow Row = (GridViewRow)TableCell.Parent;
            Grid_Documentos_Seguimiento.SelectedIndex = Row.RowIndex;
            int Fila = Row.RowIndex;

            Negocio_Solicitud.P_Solicitud_ID = HDN_Solicitud_ID.Value;
            Negocio_Solicitud = Negocio_Solicitud.Consultar_Datos_Solicitud();

            GridViewRow selectedRow = Grid_Documentos_Seguimiento.Rows[Grid_Documentos_Seguimiento.SelectedIndex];
            String Documentos = HttpUtility.HtmlDecode(selectedRow.Cells[0].Text).ToString();

            //  se borraran el archivo
            String[] Archivos = Directory.GetFiles(MapPath("../../Archivos/" + Negocio_Solicitud.P_Clave_Solicitud.Trim() + "/"));

            for (Int32 Contador = 0; Contador < Archivos.Length; Contador++)
            {
                Nombre_Archivo = Path.GetFileName(Archivos[Contador].Trim());

                if (Nombre_Archivo == Documentos)
                {
                    System.IO.File.Delete(Archivos[Contador].Trim());
                    break;
                }

            }// fin del for

            Cargar_Documentos_Seguimiento(Negocio_Solicitud, Rol_Director_Ordenamiento, Director_Ambiental, Director_Fraccionamientos,Director_Urbanistico);
        }

        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }      
    
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Crear_Documento_Click
    ///DESCRIPCIÓN: Se maneja el evento del boton de crear documento de una plantilla.
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 18/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///******************************************************************************* 
    protected void Btn_Crear_Documento_Click(object sender, EventArgs e)
    {
        try
        {
            if (Validar_Plantilla())
            {
                String Plantilla_Seleccionada = Hdf_Plantilla_Seleccionada.Value;
                String Archivo = "";
                for (Int32 Contador = 0; Contador < Grid_Plantillas.Rows.Count; Contador++)
                {
                    if (Plantilla_Seleccionada.Equals(Grid_Plantillas.Rows[Contador].Cells[1].Text.Trim()))
                    {
                        Archivo = Grid_Plantillas.Rows[Contador].Cells[3].Text.Trim();
                        break;
                    }
                }
                Cls_Interaccion_Word Word = new Cls_Interaccion_Word();
                Word.P_Documento_Origen = MapPath("../../Plantillas_Word/" + Archivo);
                Word.Iniciar_Aplicacion();
                for (Int32 Contador = 0; Contador < Grid_Marcadores_Platilla.Rows.Count; Contador++)
                {
                    TextBox Text_Temporal = (TextBox)Grid_Marcadores_Platilla.Rows[Contador].FindControl("Txt_Valor_Marcador");
                    String Marcador_ID = Grid_Marcadores_Platilla.Rows[Contador].Cells[0].Text;
                    Word.Escribir_Sobre_Marcador(Marcador_ID, Text_Temporal.Text.Trim());
                }
                Word.P_Documento_Destino = Server.MapPath("../../Archivos/" + Txt_Clave_Solicitud.Text.Trim() + "/SUB_" + Hdf_Subproceso_ID.Value.Trim() + " - " + Archivo);
                Word.Guardar_Nuevo_Documento();
                Word.Cerrar_Aplicacion();
                for (Int32 Contador = 0; Contador < Grid_Plantillas.Rows.Count; Contador++)
                {
                    if (Grid_Plantillas.Rows[Contador].Cells[1].Text.Equals(Hdf_Plantilla_Seleccionada.Value))
                    {
                        System.Web.UI.WebControls.CheckBox Check_Temporal = (System.Web.UI.WebControls.CheckBox)Grid_Plantillas.Rows[Contador].FindControl("Chk_Realizado");
                        Check_Temporal.Checked = true;
                        break;
                    }
                }
                Grid_Marcadores_Platilla.DataSource = new DataTable();
                Grid_Marcadores_Platilla.DataBind();
                UpPnl_Plantilla.Triggers.Clear();
                Hdf_Plantilla_Seleccionada.Value = "";
                MPE_Crear_Plantilla.Hide();
            }
            else
            {
                Lbl_Error_MPE_Crear_Plantilla.Text = "Faltan Datos de Llenar para este documento!!";
                Lbl_Error_MPE_Crear_Plantilla.Visible = true;
                MPE_Crear_Plantilla.Show();
            }
        }
        catch (Exception ex)
        {
            Lbl_Error_MPE_Crear_Plantilla.Text = "Btn_Crear_Documento_Click_ " + ex.Message.ToString();
            Lbl_Error_MPE_Crear_Plantilla.Visible = true;
            throw new Exception(ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Ver_Documento_Click
    ///DESCRIPCIÓN: Se maneja el evento del boton de crear documento de una plantilla.
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 19/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///******************************************************************************* 
    protected void Btn_Ver_Documento_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (sender != null)
            {
                ImageButton Boton = (ImageButton)sender;
                String Documento = Boton.CommandArgument;
                String URL = null;
                for (Int32 Contador = 0; Contador < Grid_Documentos_Tramite.Rows.Count; Contador++)
                {
                    if (Grid_Documentos_Tramite.Rows[Contador].Cells[0].Text.Equals(Documento))
                    {
                        //URL = Server.MapPath("../../Archivos/" + Txt_Clave_Solicitud.Text + "/" + Path.GetFileName(Grid_Documentos_Tramite.Rows[Contador].Cells[3].Text));
                        URL = "../../Archivos/" + "TR-" + HDN_Solicitud_ID.Value + "/" + HttpUtility.HtmlDecode(Path.GetFileName(Grid_Documentos_Tramite.Rows[Contador].Cells[3].Text));
                        break;
                    }
                }
                if (URL != null)
                {
                    Mostrar_Archivo(URL);
                }
            }

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Ver_Documento_Seguimiento_Click
    ///DESCRIPCIÓN: Se maneja el evento para ver el documento creado dentro del seguimiento.
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 20/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///******************************************************************************* 
    protected void Btn_Ver_Documento_Seguimiento_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (sender != null)
            {
                ImageButton Boton = (ImageButton)sender;
                String Documento = Boton.CommandArgument;
                String URL = null;

                System.Web.UI.WebControls.TableCell Celda = (System.Web.UI.WebControls.TableCell)Boton.Parent;
                GridViewRow Renglon = (GridViewRow)Celda.Parent;
                Grid_Documentos_Seguimiento.SelectedIndex = Renglon.RowIndex;

                for (Int32 Contador = 0; Contador < Grid_Documentos_Seguimiento.Rows.Count; Contador++)
                {
                    if (HttpUtility.HtmlDecode(Grid_Documentos_Seguimiento.Rows[Contador].Cells[0].Text).Equals(HttpUtility.HtmlDecode(Documento)))
                    {
                        URL = "../../Archivos/" + Txt_Clave_Solicitud.Text.Trim() + "/" + Path.GetFileName(HttpUtility.HtmlDecode(Grid_Documentos_Seguimiento.Rows[Contador].Cells[1].Text));
                        break;
                    }
                } if (URL != null)
                {
                    if (System.IO.Path.GetExtension(AFU_Subir_Archivo.FileName) == ".doc") 
                        Mostrar_Archivo_Word(URL);
                    else
                        Mostrar_Archivo(URL);
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Copiar_Click
    ///DESCRIPCIÓN         : Se crea una nueva solicitud del mismo tramite apartir de una existente
    ///PARAMETROS          :     
    ///CREO                : Salvador Vazquez Camacho
    ///FECHA_CREO          : 11/Julio/2012 
    ///MODIFICO            :
    ///FECHA_MODIFICO      :
    ///CAUSA_MODIFICACIÓN  :
    ///******************************************************************************* 
    protected void Btn_Copiar_Click(object sender, ImageClickEventArgs e)
    {
        Cls_Ope_Bandeja_Tramites_Negocio Negocio = new Cls_Ope_Bandeja_Tramites_Negocio();
        Negocio.P_Solicitud_ID = Hdf_Solicitud_ID.Value;
        Negocio = Negocio.Consultar_Datos_Solicitud();

        Cls_Ope_Solicitud_Tramites_Negocio Solicitud_Negocio = new Cls_Ope_Solicitud_Tramites_Negocio();
        Solicitud_Negocio.P_Tramite_ID = Negocio.P_Tramite_id;
        Solicitud_Negocio.P_Estatus = "PENDIENTE";
        Solicitud_Negocio.P_Porcentaje = "0";

        Solicitud_Negocio.P_Solicitud_ID = Negocio.P_Solicitud_ID;
        DataSet Ds_Datos = Solicitud_Negocio.Consultar_Datos_Solicitud();
        if (Ds_Datos.Tables[0].Rows.Count > 0)
        {
            String[,] Datos = new String[Ds_Datos.Tables[0].Rows.Count, 2];
            for (int i = 0; i < Ds_Datos.Tables[0].Rows.Count; i++)
            {
                Datos[i, 0] = Ds_Datos.Tables[0].Rows[i][Ope_Tra_Datos.Campo_Dato_ID].ToString();
                Datos[i, 1] = Ds_Datos.Tables[0].Rows[i][Ope_Tra_Datos.Campo_Valor].ToString();
            }
            Solicitud_Negocio.P_Datos = Datos;
        }
        else
            Solicitud_Negocio.P_Documentos = new String[0, 0];

        DataSet Ds_Documentos = Solicitud_Negocio.Consultar_Documentos_Solicitud();
        if (Ds_Documentos.Tables[0].Rows.Count > 0)
        {
            String[,] Documentos = new String[Ds_Documentos.Tables[0].Rows.Count, 2];
            for (int i = 0; i < Ds_Documentos.Tables[0].Rows.Count; i++)
            {
                Documentos[i, 0] = Ds_Documentos.Tables[0].Rows[i][Ope_Tra_Documentos.Campo_Detalle_Documento_ID].ToString();
                Documentos[i, 1] = Ds_Documentos.Tables[0].Rows[i][Ope_Tra_Documentos.Campo_URL].ToString();
            }
            Solicitud_Negocio.P_Documentos = Documentos;
        }
        else
            Solicitud_Negocio.P_Documentos = new String[0, 0];


        DataSet Ds_Solicitudes = Solicitud_Negocio.Consultar_Tramites();
        DateTime Fecha_Solucion;
        var Dias_Inhabilies = new Cls_Ope_Pre_Dias_Inhabiles_Negocio();
        Fecha_Solucion = Dias_Inhabilies.Calcular_Fecha("" + DateTime.Today, Ds_Solicitudes.Tables[0].Rows[0][Cat_Tra_Tramites.Campo_Tiempo_Estimado].ToString());
        Solicitud_Negocio.P_Fecha_Entrega = Fecha_Solucion;

        Solicitud_Negocio.P_Clave_Solicitud = Negocio.P_Clave_Solicitud;
        Ds_Solicitudes = Solicitud_Negocio.Consultar_Solicitud();
        Solicitud_Negocio.P_Clave_Solicitud = Cls_Util.Generar_Folio_Tramite();
        Solicitud_Negocio.P_Nombre_Solicitante = Ds_Solicitudes.Tables[0].Rows[0][Ope_Tra_Solicitud.Campo_Nombre_Solicitante].ToString();
        Solicitud_Negocio.P_Apellido_Paterno = Ds_Solicitudes.Tables[0].Rows[0][Ope_Tra_Solicitud.Campo_Apellido_Paterno].ToString();
        Solicitud_Negocio.P_Apellido_Materno = Ds_Solicitudes.Tables[0].Rows[0][Ope_Tra_Solicitud.Campo_Apellido_Materno].ToString();
        Solicitud_Negocio.P_E_Mail = Ds_Solicitudes.Tables[0].Rows[0][Ope_Tra_Solicitud.Campo_Correo_Electronico].ToString();
        Solicitud_Negocio.P_Comentarios = Ds_Solicitudes.Tables[0].Rows[0][Ope_Tra_Solicitud.Campo_Comentarios].ToString();
        Solicitud_Negocio.P_Cuenta_Predial = Ds_Solicitudes.Tables[0].Rows[0][Ope_Tra_Solicitud.Campo_Cuenta_Predial].ToString();
        Solicitud_Negocio.P_Inspector_ID = Ds_Solicitudes.Tables[0].Rows[0][Ope_Tra_Solicitud.Campo_Inspector_ID].ToString();

        DataSet Ds_Subproceso = Solicitud_Negocio.Consultar_Subproceso();
        Solicitud_Negocio.P_Subproceso_ID = Ds_Subproceso.Tables[0].Rows[0][Cat_Tra_Subprocesos.Campo_Subproceso_ID].ToString();
        Negocio.P_Comentarios = "Registro generado apartir de la solicitud con clave: " + Negocio.P_Clave_Solicitud;

        Solicitud_Negocio.Alta_Solicitud(Cls_Sessiones.Nombre_Empleado);

        Negocio.P_Solicitud_ID = Solicitud_Negocio.P_Solicitud_ID;
        Negocio.P_Subproceso_ID = Solicitud_Negocio.P_Subproceso_ID;
        Negocio.P_Estatus = Solicitud_Negocio.P_Estatus;
        Negocio.Alta_Detalles_Solicitud(Negocio);


        Cls_Ope_Solicitud_Tramites_Negocio Rs_Alta = new Cls_Ope_Solicitud_Tramites_Negocio();
        String Raiz = @Server.MapPath("../../Archivos");
        String Directorio_Expediente = "TR-" + Solicitud_Negocio.P_Solicitud_ID;

        String Nombre_Archivo = "";

        if (!Directory.Exists(Raiz + Directorio_Expediente))
            Directory.CreateDirectory(Raiz + "/" + Directorio_Expediente);

        String[] Archivos = Directory.GetFiles(MapPath("../../Archivos/TR-" + Hdf_Solicitud_ID.Value + "/"));

        for (Int32 Contador = 0; Contador < Archivos.Length; Contador++)
        {
            Nombre_Archivo = Path.GetFileName(Archivos[Contador].Trim());


            System.IO.File.Copy(Raiz + "/TR-" + Hdf_Solicitud_ID.Value + "/" + Nombre_Archivo, Raiz + "/" + Directorio_Expediente + "/" + Nombre_Archivo);


        }// fin del for

    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Agregar_Solicitud_Click
    ///DESCRIPCIÓN         : Se Agreaga una nueva solicitud a una existente
    ///PARAMETROS          :     
    ///CREO                : Salvador Vazquez Camacho
    ///FECHA_CREO          : 11/Julio/2012 
    ///MODIFICO            :
    ///FECHA_MODIFICO      :
    ///CAUSA_MODIFICACIÓN  :
    ///******************************************************************************* 
    protected void Btn_Agregar_Solicitud_Click(object sender, ImageClickEventArgs e)
    {
        Cls_Ope_Solicitud_Tramites_Negocio Solicitud_Negocio = new Cls_Ope_Solicitud_Tramites_Negocio();
        Cls_Ope_Bandeja_Tramites_Negocio Neg_Solicitud = new Cls_Ope_Bandeja_Tramites_Negocio();
        DataTable Dt_Datos_Dictaminar = new DataTable();
        try
        { 
            Neg_Solicitud.P_Solicitud_ID = Hdf_Solicitud_ID.Value;
            Neg_Solicitud = Neg_Solicitud.Consultar_Datos_Solicitud();


            Solicitud_Negocio.P_Solicitud_ID = Hdf_Solicitud_ID.Value;
            DataSet Ds_Solicitudes = Solicitud_Negocio.Consultar_Solicitud();
            Solicitud_Negocio.P_Nombre_Solicitante = Ds_Solicitudes.Tables[0].Rows[0][Ope_Tra_Solicitud.Campo_Nombre_Solicitante].ToString();
            Solicitud_Negocio.P_Apellido_Paterno = Ds_Solicitudes.Tables[0].Rows[0][Ope_Tra_Solicitud.Campo_Apellido_Paterno].ToString();
            Solicitud_Negocio.P_Apellido_Materno = Ds_Solicitudes.Tables[0].Rows[0][Ope_Tra_Solicitud.Campo_Apellido_Materno].ToString();
            Solicitud_Negocio.P_E_Mail = Ds_Solicitudes.Tables[0].Rows[0][Ope_Tra_Solicitud.Campo_Correo_Electronico].ToString();
            Solicitud_Negocio.P_Comentarios = Ds_Solicitudes.Tables[0].Rows[0][Ope_Tra_Solicitud.Campo_Comentarios].ToString();
            Solicitud_Negocio.P_Cuenta_Predial = Ds_Solicitudes.Tables[0].Rows[0][Ope_Tra_Solicitud.Campo_Cuenta_Predial].ToString();
            Solicitud_Negocio.P_Inspector_ID = Ds_Solicitudes.Tables[0].Rows[0][Ope_Tra_Solicitud.Campo_Inspector_ID].ToString();
            Solicitud_Negocio.P_Contribuyente_ID = Ds_Solicitudes.Tables[0].Rows[0][Ope_Tra_Solicitud.Campo_Contribuyente_Id].ToString();
            Solicitud_Negocio.P_Direccion_Predio = Ds_Solicitudes.Tables[0].Rows[0][Ope_Tra_Solicitud.Campo_Direccion_Predio].ToString();
            Solicitud_Negocio.P_Propietario_Predio = Ds_Solicitudes.Tables[0].Rows[0][Ope_Tra_Solicitud.Campo_Propietario_Predio].ToString();
            Solicitud_Negocio.P_Calle_Predio = Ds_Solicitudes.Tables[0].Rows[0][Ope_Tra_Solicitud.Campo_Calle_Predio].ToString();
            Solicitud_Negocio.P_Nuemro_Predio = Ds_Solicitudes.Tables[0].Rows[0][Ope_Tra_Solicitud.Campo_Numero_Predio].ToString();
            Solicitud_Negocio.P_Manzana_Predio = Ds_Solicitudes.Tables[0].Rows[0][Ope_Tra_Solicitud.Campo_Manzana_Predio].ToString();
            Solicitud_Negocio.P_Lote_Predio = Ds_Solicitudes.Tables[0].Rows[0][Ope_Tra_Solicitud.Campo_Lote_Predio].ToString();
            Solicitud_Negocio.P_Otros_Predio = Ds_Solicitudes.Tables[0].Rows[0][Ope_Tra_Solicitud.Campo_Otros_Predio].ToString();
            Solicitud_Negocio.P_Consecutivo = Neg_Solicitud.P_Consecutivo;

            Solicitud_Negocio.P_Comentarios = "Registro generado apartir de la solicitud con clave: " + Solicitud_Negocio.P_Clave_Solicitud;
            Solicitud_Negocio.P_Estatus = "PROCESO";
            Solicitud_Negocio.P_Porcentaje = "0";
            Solicitud_Negocio.P_Cantidad = "1";
            Solicitud_Negocio.P_Clave_Solicitud = Cls_Util.Generar_Folio_Tramite();
            Solicitud_Negocio.P_Empleado_ID = Cls_Sessiones.Empleado_ID;

            Solicitud_Negocio.P_Tramite_ID = Cmb_Agregar_Solicitud.SelectedValue;
            DataSet Ds_Tramites = Solicitud_Negocio.Consultar_Tramites();
            DataSet Ds_Subproceso = Solicitud_Negocio.Consultar_Subproceso();
            Solicitud_Negocio.P_Tramite_ID = Ds_Tramites.Tables[0].Rows[0].ItemArray[0].ToString();
            Solicitud_Negocio.P_Folio = Ds_Tramites.Tables[0].Rows[0].ItemArray[2].ToString();
            Solicitud_Negocio.P_Costo_Base = Solicitud_Negocio.P_Costo_Total = Ds_Tramites.Tables[0].Rows[0].ItemArray[7].ToString();
            Solicitud_Negocio.P_Subproceso_ID = Ds_Subproceso.Tables[0].Rows[0].ItemArray[0].ToString();
            var Dias_Inhabilies = new Cls_Ope_Pre_Dias_Inhabiles_Negocio();
            Solicitud_Negocio.P_Fecha_Entrega = Dias_Inhabilies.Calcular_Fecha("" + DateTime.Today, Ds_Tramites.Tables[0].Rows[0].ItemArray[6].ToString());
            Solicitud_Negocio.P_Complemento = Hdf_Solicitud_ID.Value;
            Solicitud_Negocio.P_Datos = new String[0, 0];
            Solicitud_Negocio.P_Documentos = new String[0, 0];

            Dt_Datos_Dictaminar = Solicitud_Negocio.Consultar_Datos_Finales_Tramite();
            String[,] Datos = Obtener_Datos_Finales_Replica_Solicitud(Dt_Datos_Dictaminar);
            Solicitud_Negocio.P_Datos = Datos;

            Solicitud_Negocio.Alta_Solicitud(Cls_Sessiones.Nombre_Empleado);
            
            Solicitud_Negocio.P_Solicitud_ID = Hdf_Solicitud_ID.Value;
            Solicitud_Negocio.P_Subproceso_ID = Neg_Solicitud.P_Subproceso_ID;
            Solicitud_Negocio.P_Estatus = "PROCESO";
            Solicitud_Negocio.Modificar_Actividad_Solicitud_Hija();

            Grid_Bandeja_Entrada_SelectedIndexChanged(sender, null);
        }
        catch (Exception Ex)
        {
            Mensaje_Error(true, Ex.ToString());
        }

    }
    
    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Cmb_Zonas_SelectedIndexChanged
    ///DESCRIPCIÓN: Manejo del evento cambio de índice en el combo zonas, consultar el empleado 
    ///             encargado de la zona seleccionada y seleccionarlo en el combo supervisor
    ///PARÁMETROS:
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 16-jul-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Cmb_Zonas_SelectedIndexChanged(object sender, EventArgs e)
    {
        Lbl_Mensaje_Error.Text = "";
        Div_Contenedor_Msj_Error.Visible = false;

        try
        {
            if (Cmb_Zonas.SelectedIndex > 0)
            {
                Cls_Cat_Ort_Zona_Negocio Neg_Consulta_Zona = new Cls_Cat_Ort_Zona_Negocio();
                DataTable Dt_Zona = null;
                Neg_Consulta_Zona.P_Zona_ID = Cmb_Zonas.SelectedValue;
                Dt_Zona = Neg_Consulta_Zona.Consultar_Zonas();
                // si la consulta regresó valores, seleccionar el supervisor de la zona seleccionada
                if (Dt_Zona != null && Dt_Zona.Rows.Count > 0)
                {
                    string Supervisor_ID = Dt_Zona.Rows[0][Cat_Ort_Zona.Campo_Empleado_ID].ToString();
                    // validar que el elemento exista en el combo, si no, agregarlo
                    if (Cmb_Supervisor_Zona.Items.FindByValue(Supervisor_ID) != null)
                    {
                        Cmb_Supervisor_Zona.SelectedValue = Supervisor_ID;
                    }
                    else
                    {
                        Cmb_Supervisor_Zona.Items.Add(new System.Web.UI.WebControls.ListItem(Dt_Zona.Rows[0][Cat_Ort_Zona.Campo_Responsable_Zona].ToString(), Supervisor_ID));
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Text = ex.Message;
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Cmb_Condicion_SelectedIndexChanged
    ///DESCRIPCIÓN: Manejo del evento cambio de índice en el combo condicion, consultar el nombre 
    ///             de la actividad a la que procede
    ///PARÁMETROS:
    ///CREO: Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO: 27-jul-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Cmb_Condicion_SelectedIndexChanged(object sender, EventArgs e)
    {
        Lbl_Mensaje_Error.Text = "";
        Div_Contenedor_Msj_Error.Visible = false;
        DataTable Dt_Actividad = new DataTable();
        Cls_Ope_Bandeja_Tramites_Negocio Negocio_Activiad = new Cls_Ope_Bandeja_Tramites_Negocio(); 
        Cls_Ope_Bandeja_Tramites_Negocio Solicitud = new Cls_Ope_Bandeja_Tramites_Negocio();
        try
        {
            //Txt_Nombre_Actividad_Condicion.Text = "";
            Solicitud.P_Solicitud_ID = HDN_Solicitud_ID.Value;
            //  se carga los datos de la consulta en la capa de negocios
            Solicitud = Solicitud.Consultar_Datos_Solicitud(); 

            if (Cmb_Condicion.SelectedValue == "SI")
                Negocio_Activiad.P_Condicion_Si = Convert.ToDouble(Hdf_Condicion_Si.Value);
            else
                Negocio_Activiad.P_Condicion_No = Convert.ToDouble(Hdf_Condicion_No.Value);
           
            Negocio_Activiad.P_Tramite_id = Hdf_Tramite_Id.Value;
            
            Dt_Actividad = Negocio_Activiad.Consultar_Actividad_Condicional();

            if (Dt_Actividad != null && Dt_Actividad.Rows.Count > 0)
            {
                Txt_Nombre_Actividad_Condicion.Text = Dt_Actividad.Rows[0][Cat_Tra_Subprocesos.Campo_Nombre].ToString();
                //Txt_Numero_Actividad.Text = Dt_Actividad.Rows[0][Cat_Tra_Subprocesos.Campo_Orden].ToString();
                //Txt_Siguiente_Subproceso.Text = Dt_Actividad.Rows[0][Cat_Tra_Subprocesos.Campo_Nombre].ToString();
            }
            else
            {
                Txt_Nombre_Actividad_Condicion.Text = "FIN DEL PROCESO DE LA SOLICITUD " + Solicitud.P_Clave_Solicitud;
                //Txt_Numero_Actividad.Text = Solicitud.P_Clave_Solicitud;
                //Txt_Siguiente_Subproceso.Text = "FIN DEL PROCESO DE LA SOLICITUD " + Solicitud.P_Clave_Solicitud;
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Text = ex.Message;
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Cmb_Buscar_Solicitudes_Estatus_SelectedIndexChanged
    ///DESCRIPCIÓN: realizara la consulta de las solicitudes
    ///PARÁMETROS:
    ///CREO: Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO: 27-jul-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Cmb_Buscar_Solicitudes_Estatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //  se ejectua el metodo del boton buscar solicitud
            Btn_Buscar_Solicitudes_Estatus_Click(sender, null);
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Text = ex.Message;
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }
    
    #endregion

    #region FileUpload

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: AFU_Subir_Archivo_UploadedComplete
    /// DESCRIPCION :subira el documento a su carpeta
    /// PARAMETROS  : 
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 24/Mayo/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void AFU_Subir_Archivo_UploadedComplete(object sender, AsyncFileUploadEventArgs e)
    {
        Cls_Ope_Bandeja_Tramites_Negocio Solicitud = new Cls_Ope_Bandeja_Tramites_Negocio();
        String Raiz = "";
        String Direccion_Archivo = "";
        try
        {
            Solicitud.P_Solicitud_ID = HDN_Solicitud_ID.Value;
            Solicitud = Solicitud.Consultar_Datos_Solicitud(); // Se obtienen los Datos a Detalle de la Solicitud Seleccionada

            if (AFU_Subir_Archivo.HasFile)
            {
                String Extension = System.IO.Path.GetExtension(AFU_Subir_Archivo.FileName);
                //if (Extension == ".pdf" || Extension == ".jpg" || Extension == ".jpeg")
                //{
                Raiz = @Server.MapPath("../../Archivos/" + Solicitud.P_Clave_Solicitud.Trim());

                if (!Directory.Exists(Raiz))
                {
                    Directory.CreateDirectory(Raiz);
                }

                Direccion_Archivo = Raiz + "/" + AFU_Subir_Archivo.FileName;
                if (AFU_Subir_Archivo.HasFile)
                {
                    //se guarda el archivo
                    AFU_Subir_Archivo.SaveAs(Direccion_Archivo);

                }
                // fin del if (AFU_Subir_Archivo.HasFile)
                //}

            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    #endregion
}