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
using System.Collections.Generic;
using Presidencia.Ordenamiento_Territorial_Zonas.Negocio;
using Presidencia.Tramites_Perfiles_Empleados.Negocio;
using Presidencia.Empleados.Negocios;
using Presidencia.Areas.Negocios;
using Presidencia.Orden_Territorial_Bitacora_Documentos.Negocio;
using System.Text;
using Presidencia.Catalogo_Ordenamiento_Territorial_Parametros.Negocio;

public partial class paginas_Ordenamiento_Territorial_Frm_Cat_Ort_Zona : System.Web.UI.Page
{
    #region Load
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Page_Load
    /// DESCRIPCION : 
    /// PARAMETROS  : 
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 12/Junio/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                Inicializar_Controles();
                ViewState["SortDirection"] = "DESC";
            }
            
            //string Ventana_Modal = "Abrir_Ventana_Modal('../Tramites/Ventanas_Emergente/Frm_Busqueda_Avanzada_Empleado.aspx', 'center:yes;resizable:no;status:no;dialogWidth:580px;dialogHeight:500px;dialogHide:true;help:no;scroll:no');";
            //Btn_Buscar_Empleado.Attributes.Add("onclick", Ventana_Modal);

            
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }
    #endregion

    #region Metodos generales
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Inicializa_Controles
    /// DESCRIPCION : Prepara los controles en la forma para que el usuario pueda
    ///               realizar diferentes operaciones
    /// PARAMETROS  : 
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 12/Junio/2012
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
            Cargar_Zonas();
            Cargar_Supervisor();
            Cargar_Areas();
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
    /// FECHA_CREO  : 12/Junio/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Limpiar_Controles()
    {
        try
        {
            Hdf_Elemento_ID.Value = "";
            Txt_Busqueda.Text = "";
            Txt_Nombre.Text = "";

        }
        catch (Exception ex)
        {
            throw new Exception("Limpia_Controles " + ex.Message.ToString());
        }
    }
    ///*******************************************************************************
    ///NOMBRE:          Llenar_Combo_Supervisor
    ///DESCRIPCIÓN:     Llenara el combo de los perfiles
    ///PARAMETROS: 
    ///CREO:            Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:      25/Mayo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Llenar_Combo_Supervisor(DataTable Dt_Consulta)
    {
        try
        {
            Cmb_Responsable_Empleado.DataSource = Dt_Consulta;
            Cmb_Responsable_Empleado.DataValueField = Cat_Empleados.Campo_Empleado_ID;
            Cmb_Responsable_Empleado.DataTextField = "Nombre_Usuario";
            Cmb_Responsable_Empleado.DataBind();
            Cmb_Responsable_Empleado.Items.Insert(0, "< SELECCIONE >");

        }
        catch (Exception ex)
        {
            throw new Exception("Llenar_Combo_Empleados " + ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE:          Llenar_Combo_Supervisor
    ///DESCRIPCIÓN:     Llenara el combo de los perfiles
    ///PARAMETROS: 
    ///CREO:            Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:      25/Mayo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Llenar_Combo_Areas(DataTable Dt_Consulta)
    {
        try
        {
            Cmb_Area.DataSource = Dt_Consulta;
            Cmb_Area.DataValueField = Cat_Areas.Campo_Area_ID;
            Cmb_Area.DataTextField = "NOMBRE";
            Cmb_Area.DataBind();
            Cmb_Area.Items.Insert(0, "< SELECCIONE >");

        }
        catch (Exception ex)
        {
            throw new Exception("Llenar_Combo_Empleados " + ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    /// NOMBRE:         Habilitar_Controles
    /// DESCRIPCION :   Habilita y Deshabilita los controles de la forma para prepara la página
    ///                 para a siguiente operación
    /// PARAMETROS:     1.- Operacion: Indica la operación que se desea realizar 
    /// CREO:           Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO:     12/Junio/2012
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
                    Btn_Modificar.ToolTip = "Modificar";
                    Btn_Salir.ToolTip = "Salir";
                    Btn_Nuevo.Visible = true;
                    Btn_Modificar.Visible = true;
                    Btn_Eliminar.Visible = true;
                    Btn_Nuevo.CausesValidation = false;
                    Btn_Modificar.CausesValidation = false;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                    break;

                case "Nuevo":
                    Habilitado = true;
                    Btn_Nuevo.ToolTip = "Dar de Alta";
                    Btn_Modificar.ToolTip = "Modificar";
                    Btn_Salir.ToolTip = "Cancelar";
                    Btn_Nuevo.Visible = true;
                    Btn_Modificar.Visible = false;
                    Btn_Eliminar.Visible = false;
                    Btn_Nuevo.CausesValidation = true;
                    Btn_Modificar.CausesValidation = true;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                    break;

                case "Modificar":
                    Habilitado = true;
                    Btn_Nuevo.ToolTip = "Nuevo";
                    Btn_Modificar.ToolTip = "Actualizar";
                    Btn_Salir.ToolTip = "Cancelar";
                    Btn_Nuevo.Visible = false;
                    Btn_Modificar.Visible = true;
                    Btn_Eliminar.Visible = false;
                    Btn_Nuevo.CausesValidation = true;
                    Btn_Modificar.CausesValidation = true;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                    break;
            }
            //  mensajes de error
            Mostrar_Mensaje_Error(false);

            Txt_Nombre.Enabled = Habilitado;
            Cmb_Responsable_Empleado.Enabled = Habilitado;
            Cmb_Area.Enabled = Habilitado;
            //Btn_Buscar_Empleado.Enabled = Habilitado;

            Txt_Busqueda.Enabled = !Habilitado;
            Btn_Buscar.Enabled = !Habilitado;
            Grid_Zona.Enabled = !Habilitado;
        }
        catch (Exception ex)
        {
            throw new Exception("Habilitar_Controles " + ex.Message.ToString());
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Mostrar_Mensaje_Error
    ///DESCRIPCIÓN          : se habilitan los mensajes de error
    ///PARAMETROS           : 
    ///CREO                 : Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO           : 12/Junio/2012
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
            throw new Exception("Mostrar_Mensaje_Error " + ex.Message.ToString());
        }
    }
    #endregion

    #region Validaciones
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Validar_Datos
    /// DESCRIPCION : Validar que se se encuentre todos los datos para continuar con el proceso
    /// PARAMETROS: 
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 12/Junio/2012
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

        if (Txt_Nombre.Text == "")
        {
            Lbl_Mensaje_Error.Text += Espacios_Blanco + "*El nombre.<br>";
            Datos_Validos = false;
        }
        if (Cmb_Responsable_Empleado.SelectedIndex == 0)
        {
            Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Seleccione al responsable de la zona.<br>";
            Datos_Validos = false;
        }
        if (Cmb_Area.SelectedIndex == 0)
        {
            Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Seleccione el área.<br>";
            Datos_Validos = false;
        }
        return Datos_Validos;
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Validar_Datos_Modificar
    /// DESCRIPCION : Validar que se se encuentre todos los datos para continuar con el proceso
    /// PARAMETROS: 
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 12/Junio/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private Boolean Validar_Datos_Modificar()
    {
        String Espacios_Blanco = "";
        Boolean Datos_Validos = true;//Variable que almacena el valor de true si todos los datos fueron ingresados de forma correcta, o false en caso contrario.
        Lbl_Mensaje_Error.Text = "";
        Espacios_Blanco = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
        Mostrar_Mensaje_Error(true);

        if (Hdf_Elemento_ID.Value == "")
        {
            Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Seleccione algun registro.<br>";
            Datos_Validos = false;
        }
        if (Txt_Nombre.Text == "")
        {
            Lbl_Mensaje_Error.Text += Espacios_Blanco + "*El nombre.<br>";
            Datos_Validos = false;
        }
        if (Cmb_Responsable_Empleado.SelectedIndex == 0)
        {
            Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Seleccione al responsable de la zona.<br>";
            Datos_Validos = false;
        }
        if (Cmb_Area.SelectedIndex == 0)
        {
            Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Seleccione el área.<br>";
            Datos_Validos = false;
        }

        return Datos_Validos;
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Cargar_Zonas
    ///DESCRIPCIÓN          : se cargara los datos de la tabla
    ///PARAMETROS           : 
    ///CREO                 : Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO           : 12/Junio/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    private void Cargar_Zonas()
    {
        Cls_Cat_Ort_Zona_Negocio Negocio_Consulta = new Cls_Cat_Ort_Zona_Negocio();
        DataTable Dt_Consulta = new DataTable();
        try
        { 
            Dt_Consulta = Negocio_Consulta.Consultar_Zonas();
            //  se ordena por area, nombre de la zona
            DataView Dv_Ordenar = new DataView(Dt_Consulta);
            Dv_Ordenar.Sort = Cat_Ort_Zona.Campo_Area + ", " + Cat_Ort_Zona.Campo_Nombre +" asc ";
            Dt_Consulta = Dv_Ordenar.ToTable();
            Cargar_Grid_Zonas(Dt_Consulta);
        }
        catch (Exception ex)
        {
            throw new Exception("Cargar_Zonas " + ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Cargar_Supervisor
    ///DESCRIPCIÓN          : se cargara los datos del supervisor
    ///PARAMETROS           : 
    ///CREO                 : Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO           : 17/Julio/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    private void Cargar_Supervisor()
    {
        Cls_Ope_Ort_Bitacora_Documentos_Negocio Negocio_Consultar_Solicitud = new Cls_Ope_Ort_Bitacora_Documentos_Negocio();
        DataTable Dt_Consulta = new DataTable();
        try
        {
            Dt_Consulta = Negocio_Consultar_Solicitud.Consultar_Personal();
            Llenar_Combo_Supervisor(Dt_Consulta);
        }
        catch (Exception ex)
        {
            throw new Exception("Cargar_Empleados " + ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Cargar_Supervisor
    ///DESCRIPCIÓN          : se cargara los datos del supervisor
    ///PARAMETROS           : 
    ///CREO                 : Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO           : 17/Julio/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    private void Cargar_Areas()
    {
        Cls_Cat_Areas_Negocio Negocio = new Cls_Cat_Areas_Negocio();
        DataTable Dt_Areas = new DataTable();
        StringBuilder Expresion_Sql = new StringBuilder();
        Cls_Cat_Ort_Parametros_Negocio Obj_Parametros = new Cls_Cat_Ort_Parametros_Negocio();
        string Dependencia_ID_Ordenamiento = "";
        string Dependencia_ID_Ambiental = "";
        string Dependencia_ID_Urbanistico = "";
        string Dependencia_ID_Inmobiliario = "";

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


            //  filtro para obtener las areas de los parametros de ordenamiento
            Expresion_Sql.Append(Cat_Areas.Campo_Area_ID + "='" + Dependencia_ID_Ordenamiento + "'");
            Expresion_Sql.Append(" or " + Cat_Areas.Campo_Area_ID + "='" + Dependencia_ID_Ambiental + "'");
            Expresion_Sql.Append(" or " + Cat_Areas.Campo_Area_ID + "='" + Dependencia_ID_Urbanistico + "'");
            Expresion_Sql.Append(" or " + Cat_Areas.Campo_Area_ID + "='" + Dependencia_ID_Inmobiliario + "'");

            Dt_Areas = Negocio.Consulta_Areas();

            DataRow[] Drow_Areas = Dt_Areas.Select(Expresion_Sql.ToString());

            Dt_Areas = (DataTable)(Drow_Areas.CopyToDataTable());

            Llenar_Combo_Areas(Dt_Areas);
        }
        catch (Exception ex)
        {
            throw new Exception("Cargar_Empleados " + ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Cargar_Grid_Zonas
    ///DESCRIPCIÓN          : se cargara el grid
    ///PARAMETROS           : 
    ///CREO                 : Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO           : 12/Junio/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    private void Cargar_Grid_Zonas(DataTable Dt_Consulta)
    {
        try
        {
            Session["GRID_ZONA"] = Dt_Consulta;

            if (Dt_Consulta != null && Dt_Consulta.Rows.Count > 0)
            {
                Grid_Zona.Columns[1].Visible = true;
                Grid_Zona.Columns[4].Visible = true;
                Grid_Zona.DataSource = Dt_Consulta;
                Grid_Zona.DataBind();
                Grid_Zona.Columns[1].Visible = false;
                Grid_Zona.Columns[4].Visible = false;
                Grid_Zona.SelectedIndex = -1;
            }
            else
            {
                Grid_Zona.Columns[1].Visible = true;
                Grid_Zona.Columns[4].Visible = true;
                Grid_Zona.DataSource = new DataTable();
                Grid_Zona.DataBind();
                Grid_Zona.Columns[1].Visible = false;
                Grid_Zona.Columns[4].Visible = false;
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Cargar_Grid_Zonas " + ex.Message.ToString());
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Alta
    ///DESCRIPCIÓN          : se pasaran los elementos a la capa de negocio
    ///PARAMETROS           : 
    ///CREO                 : Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO           : 12/Junio/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    private void Alta()
    {
        Cls_Cat_Ort_Zona_Negocio Negocio_Alta = new Cls_Cat_Ort_Zona_Negocio();
        try
        {
            Negocio_Alta.P_Nombre = Txt_Nombre.Text;
            Negocio_Alta.P_Usuario = Cls_Sessiones.Nombre_Empleado;
            Negocio_Alta.P_Empleado_ID = Cmb_Responsable_Empleado.SelectedValue;
            Negocio_Alta.P_Responsable_Zona = Cmb_Responsable_Empleado.SelectedItem.ToString();
            Negocio_Alta.P_Area_ID = Cmb_Area.SelectedValue;
            Negocio_Alta.P_Area = Cmb_Area.SelectedItem.ToString();
            Negocio_Alta.Alta();
        }
        catch (Exception ex)
        {
            throw new Exception("Alta " + ex.Message.ToString());
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Modificar
    ///DESCRIPCIÓN          : se pasaran los elementos a la capa de negocio
    ///PARAMETROS           : 
    ///CREO                 : Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO           : 12/Junio/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    private void Modificar()
    {
        Cls_Cat_Ort_Zona_Negocio Negocio_Modificar = new Cls_Cat_Ort_Zona_Negocio();
        try
        {
            Negocio_Modificar.P_Zona_ID = Hdf_Elemento_ID.Value;
            Negocio_Modificar.P_Nombre = Txt_Nombre.Text;
            Negocio_Modificar.P_Usuario = Cls_Sessiones.Nombre_Empleado;
            Negocio_Modificar.P_Empleado_ID = Cmb_Responsable_Empleado.SelectedValue;
            Negocio_Modificar.P_Responsable_Zona = Cmb_Responsable_Empleado.SelectedItem.ToString();
            Negocio_Modificar.P_Area_ID = Cmb_Area.SelectedValue;
            Negocio_Modificar.P_Area = Cmb_Area.SelectedItem.ToString();
            Negocio_Modificar.Modificar();
        }
        catch (Exception ex)
        {
            throw new Exception("Modificar " + ex.Message.ToString());
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Eliminar
    ///DESCRIPCIÓN          : se pasaran los elementos a la capa de negocio
    ///PARAMETROS           : 
    ///CREO                 : Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO           : 12/Junio/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    private void Eliminar()
    {
        Cls_Cat_Ort_Zona_Negocio Negocio_Eliminar = new Cls_Cat_Ort_Zona_Negocio();
        try
        {
            Negocio_Eliminar.P_Zona_ID = Hdf_Elemento_ID.Value;
            Negocio_Eliminar.Eliminar();
        }
        catch (Exception ex)
        {
            throw new Exception("Eliminar " + ex.Message.ToString());
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Buscar
    ///DESCRIPCIÓN          : se pasaran los elementos a la capa de negocio
    ///PARAMETROS           : 
    ///CREO                 : Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO           : 12/Junio/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    private void Buscar()
    {
        Cls_Cat_Ort_Zona_Negocio Negocio_Buscar = new Cls_Cat_Ort_Zona_Negocio();
        DataTable Dt_Consulta = new DataTable();
        try
        {
            Negocio_Buscar.P_Nombre = Txt_Busqueda.Text;
            Dt_Consulta = Negocio_Buscar.Consultar_Zonas();
            Cargar_Grid_Zonas(Dt_Consulta);
        }
        catch (Exception ex)
        {
            throw new Exception("Buscar " + ex.Message.ToString());
        }
    }
    #endregion

    #region Botones
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Btn_Nuevo_Click
    /// DESCRIPCION : realiza la alta del usuario
    /// PARAMETROS: 
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 12/Junio/2012
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
                Limpiar_Controles();           //Limpia los controles de la forma para poder introducir nuevos datos
                Habilitar_Controles("Nuevo"); //Habilita los controles para la introducción de datos por parte del usuario
                Cargar_Supervisor();
                Cargar_Areas();
            }
            else
            {
                if (Validar_Datos())
                {
                    Alta();
                    Inicializar_Controles();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Btn_Nuevo_Click", "alert('Alta Exitosa');", true);
                }
                else
                {
                    Mostrar_Mensaje_Error(true);
                }
            }

        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true);
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Btn_Modificar_Click
    /// DESCRIPCION : realiza la modificacion del usuario
    /// PARAMETROS: 
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 12/Junio/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Mostrar_Mensaje_Error(false);

            if (Btn_Modificar.ToolTip == "Modificar" && Hdf_Elemento_ID.Value != "")
            {
                Habilitar_Controles("Modificar"); //Habilita los controles para la introducción de datos por parte del usuario
            }
            else
            {
                if (Validar_Datos_Modificar())
                {
                    Modificar();
                    Inicializar_Controles();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Btn_Modificar_Click", "alert('Modificacion Exitosa');", true);
                }
                else
                {
                    Mostrar_Mensaje_Error(true);
                }
            }

        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true);
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Btn_Eliminar_Click
    /// DESCRIPCION : realiza la baja
    /// PARAMETROS: 
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 12/Junio/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Eliminar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Mostrar_Mensaje_Error(false);

            if (!string.IsNullOrEmpty(Hdf_Elemento_ID.Value))
            {
                Eliminar();
                Inicializar_Controles();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Btn_Eliminar_Click", "alert('Baja Exitosa');", true);
            }
            else
            {
                Mostrar_Mensaje_Error(true);
                Lbl_Mensaje_Error.Text = "Seleccione el registro que se eliminara <br>";
            }

        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true);
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Btn_Salir_Click
    /// DESCRIPCION : 
    /// PARAMETROS: 
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 12/Junio/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Btn_Salir.ToolTip == "Cancelar")
            {
                Inicializar_Controles();
            }
            else
            {
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true);
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Btn_Buscar_Click
    /// DESCRIPCION : 
    /// PARAMETROS: 
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 12/Junio/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Mostrar_Mensaje_Error(false);
            if (!String.IsNullOrEmpty(Txt_Busqueda.Text))
            {
                Buscar();
            }
            else
            {
                //Mostrar_Mensaje_Error(true);
                //Lbl_Mensaje_Error.Text = "Ingrese el nombre a buscar";
                Cargar_Zonas();
            }

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }
     ///*******************************************************************************
    ///NOMBRE:          Btn_Buscar_Empleado_Click
    ///DESCRIPCIÓN:     Realizara los metodos requeridos buscar al empleado
    ///PARAMETROS: 
    ///CREO:            Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:      07/Julio/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Buscar_Empleado_Click(object sender, EventArgs e)
    {
        DataTable Dt_Empleado = new DataTable();
        String Empleado_ID = ""; 
        try
        {
            if (Session["BUSQUEDA_EMPLEADO"] != null)
            {
                Boolean Estado = Convert.ToBoolean(Session["BUSQUEDA_EMPLEADO"].ToString());

                if (Estado != false)
                {
                    Empleado_ID = Session["EMPLEADO_ID"].ToString();
                    Dt_Empleado = (DataTable)(Session["Dt_Empleados"]);

                    Llenar_Combo_Supervisor(Dt_Empleado);
                    Cmb_Responsable_Empleado.SelectedIndex = Cmb_Responsable_Empleado.Items.IndexOf(Cmb_Responsable_Empleado.Items.FindByValue(Empleado_ID));                 
                    
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Btn_Buscar_Empleado_Click " + ex.Message.ToString(), ex);
        }
    }
    
    #endregion

    #region Grid
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Grid_Zona_SelectedIndexChanged
    /// DESCRIPCION : se cargara la informacion del grid en las cajas de texto
    /// PARAMETROS: 
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 12/Junio/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Grid_Zona_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Limpiar_Controles();
            GridViewRow selectedRow = Grid_Zona.Rows[Grid_Zona.SelectedIndex];
            Hdf_Elemento_ID.Value = HttpUtility.HtmlDecode(selectedRow.Cells[1].Text).ToString();
            Cls_Cat_Ort_Zona_Negocio Negocio = new Cls_Cat_Ort_Zona_Negocio();
            Negocio.P_Zona_ID = Hdf_Elemento_ID.Value;
            DataTable Dt_Zonas = Negocio.Consultar_Zonas();
            Txt_Nombre.Text = HttpUtility.HtmlDecode(selectedRow.Cells[2].Text).ToString();
            Cmb_Responsable_Empleado.SelectedIndex = Cmb_Responsable_Empleado.Items.IndexOf(Cmb_Responsable_Empleado.Items.FindByValue(HttpUtility.HtmlDecode(selectedRow.Cells[4].Text).ToString()));

            if (Dt_Zonas.Rows.Count > 0)
                Cmb_Area.SelectedIndex = Cmb_Area.Items.IndexOf(Cmb_Area.Items.FindByValue(Dt_Zonas.Rows[0][Cat_Ort_Zona.Campo_Area].ToString()));
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }
    ///********************************************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Grid_Zona_OnSorting
    ///DESCRIPCIÓN          : ordena las columnas
    ///PROPIEDADES          :
    ///CREO                 : Hugo Enrique Ramirez Aguilera
    /// FECHA_CREO          : 31/Julio/2012
    ///MODIFICO             :
    ///FECHA_MODIFICO       :
    ///CAUSA_MODIFICACIÓN...:
    ///*********************************************************************************************************
    protected void Grid_Zona_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable Dt_Consulta = (DataTable)(Session["GRID_ZONA"]);

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
        Grid_Zona.Columns[1].Visible = true;
        Grid_Zona.DataSource = Dv_Ordenar;
        Grid_Zona.DataBind();
        Grid_Zona.Columns[1].Visible = false;

        Session["GRID_ZONA"] = Dv_Ordenar.ToTable();

    }
    #endregion
}
