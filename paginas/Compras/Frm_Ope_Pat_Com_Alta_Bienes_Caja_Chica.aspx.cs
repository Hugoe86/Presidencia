using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Generic;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using Presidencia.Control_Patrimonial_Operacion_Bienes_Caja_Chica.Negocio;
using Presidencia.Almacen_Resguardos.Negocio;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using System.IO;

public partial class paginas_predial_Frm_Ope_Pat_Com_Alta_Bienes_Caja_Chica : System.Web.UI.Page
{


    #region Page_Load

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Page_Load
    ///DESCRIPCIÓN: Metodo que se carga cada que ocurre un PostBack de la Página
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 24/Enero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Page_Load(object sender, EventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        if (!IsPostBack)
        {
            Configuracion_Formulario(true);
            Llenar_Combo_Dependencias();
            Llenar_Combo_Materiales();
            Llenar_Combo_Colores();
            Llenar_Combo_Marcas();
            Llenar_Combo_Modelos();
            Grid_Resguardantes.Columns[1].Visible = false;
        }
    }

    #endregion

    #region Metodos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Empleados
    ///DESCRIPCIÓN: Llena el combo de Empleados.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 24/Enero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Combo_Empleados(DataTable Tabla)
    {
        try
        {
            DataRow Fila_Empleado = Tabla.NewRow();
            Fila_Empleado["EMPLEADO_ID"] = "SELECCIONE";
            Fila_Empleado["NOMBRE"] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
            Tabla.Rows.InsertAt(Fila_Empleado, 0);
            Cmb_Empleados.DataSource = Tabla;
            Cmb_Empleados.DataValueField = "EMPLEADO_ID";
            Cmb_Empleados.DataTextField = "NOMBRE";
            Cmb_Empleados.DataBind();
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Dependencias
    ///DESCRIPCIÓN: Se llena el Combo Dependencia.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 24/Enero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Combo_Dependencias()
    {
        try
        {
            Cls_Ope_Pat_Com_Bienes_Caja_Chica_Negocio Combos = new Cls_Ope_Pat_Com_Bienes_Caja_Chica_Negocio();
            Combos.P_Tipo_DataTable = "DEPENDENCIAS";
            DataTable Dependencias = Combos.Consultar_DataTable();
            DataRow Fila_Dependencia = Dependencias.NewRow();
            Fila_Dependencia["DEPENDENCIA_ID"] = "SELECCIONE";
            Fila_Dependencia["NOMBRE"] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
            Dependencias.Rows.InsertAt(Fila_Dependencia, 0);
            Cmb_Dependencias.DataSource = Dependencias;
            Cmb_Dependencias.DataValueField = "DEPENDENCIA_ID";
            Cmb_Dependencias.DataTextField = "NOMBRE";
            Cmb_Dependencias.DataBind();
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Materiales
    ///DESCRIPCIÓN: Se llena el Combo Materiales.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 11/Febrero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Combo_Materiales()
    {
        try
        {
            Cls_Ope_Pat_Com_Bienes_Caja_Chica_Negocio Combos = new Cls_Ope_Pat_Com_Bienes_Caja_Chica_Negocio();
            Combos.P_Tipo_DataTable = "MATERIALES";
            DataTable Materiales = Combos.Consultar_DataTable();
            DataRow Fila_Material = Materiales.NewRow();
            Fila_Material["MATERIAL_ID"] = "SELECCIONE";
            Fila_Material["DESCRIPCION"] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
            Materiales.Rows.InsertAt(Fila_Material, 0);
            Cmb_Material.DataSource = Materiales;
            Cmb_Material.DataValueField = "MATERIAL_ID";
            Cmb_Material.DataTextField = "DESCRIPCION";
            Cmb_Material.DataBind();
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Colores
    ///DESCRIPCIÓN: Se llena el Combo Colores.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 11/Febrero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Combo_Colores()
    {
        try
        {
            Cls_Ope_Pat_Com_Bienes_Caja_Chica_Negocio Combos = new Cls_Ope_Pat_Com_Bienes_Caja_Chica_Negocio();
            Combos.P_Tipo_DataTable = "COLORES";
            DataTable Colores = Combos.Consultar_DataTable();
            DataRow Fila_Color = Colores.NewRow();
            Fila_Color["COLOR_ID"] = "SELECCIONE";
            Fila_Color["DESCRIPCION"] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
            Colores.Rows.InsertAt(Fila_Color, 0);
            Cmb_Color.DataSource = Colores;
            Cmb_Color.DataValueField = "COLOR_ID";
            Cmb_Color.DataTextField = "DESCRIPCION";
            Cmb_Color.DataBind();
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Marcas
    ///DESCRIPCIÓN: Se llena el Combo Marcas.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 11/Febrero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Combo_Marcas()
    {
        try
        {
            Cls_Ope_Pat_Com_Bienes_Caja_Chica_Negocio Combos = new Cls_Ope_Pat_Com_Bienes_Caja_Chica_Negocio();
            Combos.P_Tipo_DataTable = "MARCAS";
            DataTable Marcas = Combos.Consultar_DataTable();
            DataRow Fila_Marca = Marcas.NewRow();
            Fila_Marca["MARCA_ID"] = "SELECCIONE";
            Fila_Marca["NOMBRE"] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
            Marcas.Rows.InsertAt(Fila_Marca, 0);
            Cmb_Marca.DataSource = Marcas;
            Cmb_Marca.DataValueField = "MARCA_ID";
            Cmb_Marca.DataTextField = "NOMBRE";
            Cmb_Marca.DataBind();
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Modelos
    ///DESCRIPCIÓN: Se llena el Combo Modelos.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 11/Febrero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Combo_Modelos()
    {
        try
        {
            Cls_Ope_Pat_Com_Bienes_Caja_Chica_Negocio Combos = new Cls_Ope_Pat_Com_Bienes_Caja_Chica_Negocio();
            Combos.P_Tipo_DataTable = "MODELOS";
            DataTable Modelos = Combos.Consultar_DataTable();
            DataRow Fila_Modelo = Modelos.NewRow();
            Fila_Modelo["MODELO_ID"] = "SELECCIONE";
            Fila_Modelo["NOMBRE"] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
            Modelos.Rows.InsertAt(Fila_Modelo, 0);
            Cmb_Modelo.DataSource = Modelos;
            Cmb_Modelo.DataValueField = "MODELO_ID";
            Cmb_Modelo.DataTextField = "NOMBRE";
            Cmb_Modelo.DataBind();
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Configuracion_Formulario
    ///DESCRIPCIÓN: Carga una configuracion de los controles del Formulario
    ///PROPIEDADES:     
    ///             1. Estatus. Estatus en el que se cargara la configuración de los 
    ///                         controles.
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 24/Enero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Configuracion_Formulario(Boolean Estatus)
    {
        if (Estatus)
        {
            Btn_Nuevo.AlternateText = "Nuevo";
            Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
            Btn_Salir.AlternateText = "Salir";
            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
        }
        else
        {
            Btn_Nuevo.AlternateText = "Dar de Alta";
            Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
            Btn_Salir.AlternateText = "Cancelar";
            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
        }
        Txt_Nombre.Enabled = !Estatus;
        Cmb_Dependencias.Enabled = !Estatus;
        Txt_Numero_Inventario.Enabled = !Estatus;
        Txt_Cantidad.Enabled = !Estatus;
        Cmb_Material.Enabled = !Estatus;
        Cmb_Color.Enabled = !Estatus;
        Cmb_Marca.Enabled = !Estatus;
        Cmb_Modelo.Enabled = !Estatus;
        Txt_Costo.Enabled = !Estatus;
        Btn_Fecha_Adquisicion.Visible = !Estatus;
        Cmb_Estado.Enabled = !Estatus;
        Txt_Comentarios_Generales.Enabled = !Estatus;
        Cmb_Empleados.Enabled = !Estatus;
        Txt_Comentarios.Enabled = !Estatus;
        Btn_Agregar_Resguardante.Visible = !Estatus;
        Btn_Quitar_Resguardante.Visible = !Estatus;
        AFU_Archivo.Enabled = !Estatus;

        Configuracion_Acceso("Frm_Ope_Pat_Com_Alta_Bienes_Caja_Chica.aspx");
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Limpiar_Catalogo_Generales
    ///DESCRIPCIÓN: Limpia los controles de detalles del Formulario
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 24/Enero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Limpiar_Catalogo_Generales()
    {
        Txt_Nombre.Text = "";
        Cmb_Dependencias.SelectedIndex = 0;
        Txt_Numero_Inventario.Text = "";
        Txt_Cantidad.Text = "";
        Cmb_Material.SelectedIndex = 0;
        Cmb_Color.SelectedIndex = 0;
        Cmb_Marca.SelectedIndex = 0;
        Cmb_Modelo.SelectedIndex = 0;
        Txt_Costo.Text = "";
        Txt_Fecha_Adquisicion.Text = "";
        Cmb_Estado.SelectedIndex = 0;
        Cmb_Estatus.SelectedIndex = 0;
        Txt_Comentarios_Generales.Text = "";
        Grid_Resguardantes.SelectedIndex = -1;
        Grid_Resguardantes.DataSource = new DataTable();
        Grid_Resguardantes.DataBind();
        Session.Remove("Dt_Resguardantes");
        Limpiar_Detalles_Resguardos();
        Remover_Sesiones_Control_AsyncFileUpload(AFU_Archivo.ClientID);
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Limpiar_Detalles_Resguardos
    ///DESCRIPCIÓN: Limpia los controles de detalles del Formulario
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 24/Enero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Limpiar_Detalles_Resguardos()
    {
        Cmb_Empleados.SelectedIndex = 0;
        Txt_Comentarios.Text = "";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Remover_Sesiones_Control_AsyncFileUpload
    ///DESCRIPCIÓN: Limpia un control de AsyncFileUpload
    ///PROPIEDADES:     
    ///CREO: Juan Alberto Hernandez Negrete
    ///FECHA_CREO: 16/Febrero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Remover_Sesiones_Control_AsyncFileUpload(String Cliente_ID)
    {
        HttpContext Contexto;
        if (HttpContext.Current != null && HttpContext.Current.Session != null)
        {
            Contexto = HttpContext.Current;
        }
        else
        {
            Contexto = null;
        }
        if (Contexto != null)
        {
            foreach (String key in Contexto.Session.Keys)
            {
                if (key.Contains(Cliente_ID))
                {
                    Contexto.Session.Remove(key);
                    break;
                }
            }
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Buscar_Clave_DataTable
    ///DESCRIPCIÓN: Busca una Clave en un DataTable, si la encuentra Retorna 'true'
    ///             en caso contrario 'false'.
    ///PROPIEDADES:  
    ///             1.  Clave.  Clave que se buscara en el DataTable
    ///             2.  Tabla.  Datatable donde se va a buscar la clave.
    ///             3.  Columna.Columna del DataTable donde se va a buscar la clave.
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 24/Enero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private Boolean Buscar_Clave_DataTable(String Clave, DataTable Tabla, Int32 Columna)
    {
        Boolean Resultado_Busqueda = false;
        if (Tabla != null && Tabla.Rows.Count > 0 && Tabla.Columns.Count > 0)
        {
            if (Tabla.Columns.Count > Columna)
            {
                for (Int32 Contador = 0; Contador < Tabla.Rows.Count; Contador++)
                {
                    if (Tabla.Rows[Contador][Columna].ToString().Trim().Equals(Clave.Trim()))
                    {
                        Resultado_Busqueda = true;
                        break;
                    }
                }
            }
        }
        return Resultado_Busqueda;
    }

    #region Grids

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Resguardantes
    ///DESCRIPCIÓN: Llena la tabla de Resguardantes
    ///PROPIEDADES:     
    ///             1.  Pagina. Pagina en la cual se mostrará el Grid_VIew
    ///             2.  tabla.  Tabla que se va a cargar en el Grid.    
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 24/Enero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Grid_Resguardantes(Int32 Pagina, DataTable Tabla)
    {
        Grid_Resguardantes.Columns[1].Visible = true;
        Grid_Resguardantes.DataSource = Tabla;
        Grid_Resguardantes.PageIndex = Pagina;
        Grid_Resguardantes.DataBind();
        Grid_Resguardantes.Columns[1].Visible = false;
        Session["Dt_Resguardantes"] = Tabla;
    }

    #endregion

    #region Validaciones

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Componentes_Generales
    ///DESCRIPCIÓN: Hace una validacion de que haya datos en los componentes antes de hacer
    ///             una operación de la parte de Generales.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 24/Enero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private Boolean Validar_Componentes_Generales()
    {
        Lbl_Ecabezado_Mensaje.Text = "Es necesario.";
        String Mensaje_Error = "";
        Boolean Validacion = true;
        if (Txt_Nombre.Text.Trim().Length == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Introducir el Nombre del Bien.";
            Validacion = false;
        }
        if (Cmb_Dependencias.SelectedIndex == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Debe seleccionar una opción del Combo de Dependencia.";
            Validacion = false;
        }
        if (Txt_Numero_Inventario.Text.Trim().Length == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Introducir el Número de Invetario.";
            Validacion = false;
        }
        if (Txt_Cantidad.Text.Trim().Length == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Introducir la Cantidad.";
            Validacion = false;
        }
        else
        {
            if (Convert.ToInt32(Txt_Cantidad.Text.Trim()) == 0)
            {
                if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                Mensaje_Error = Mensaje_Error + "+ La Cantidad debe ser de minimo 1.";
                Validacion = false;
            }
        }
        if (Cmb_Material.SelectedIndex == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Debe seleccionar una opción del Combo de Material.";
            Validacion = false;
        }
        if (Cmb_Color.SelectedIndex == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Debe seleccionar una opción del Combo de Color.";
            Validacion = false;
        }
        if (Cmb_Marca.SelectedIndex == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Debe seleccionar una opción del Combo de Marca.";
            Validacion = false;
        }
        if (Cmb_Modelo.SelectedIndex == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Debe seleccionar una opción del Combo de Modelo.";
            Validacion = false;
        }
        if (Txt_Costo.Text.Trim().Length == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Introducir el Costo.";
            Validacion = false;
        }
        if (Cmb_Estado.SelectedIndex == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Debe seleccionar una opción del Combo de Estado.";
            Validacion = false;
        }
        if (Txt_Fecha_Adquisicion.Text.Trim().Length == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Introducir el Fecha de Adquisición.";
            Validacion = false;
        }
        if (Txt_Comentarios_Generales.Text.Trim().Length > 0 && Txt_Comentarios_Generales.Text.Trim().Length > 500)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Verificar la longitud de las comentarios (Se pasa por " + (Txt_Comentarios_Generales.Text.Trim().Length - 500).ToString() + ").";
            Validacion = false;
        }
        if (Grid_Resguardantes.Rows.Count == 0 || Session["Dt_Resguardantes"] == null)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Debe haber como minimo un empleado para resguardo.";
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
    ///NOMBRE DE LA FUNCIÓN: Validar_Componentes_Resguardos
    ///DESCRIPCIÓN: Hace una validacion de que haya datos en los componentes antes de hacer
    ///             una operación de la parte de Resguardos.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 24/Enero/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private Boolean Validar_Componentes_Resguardos()
    {
        Lbl_Ecabezado_Mensaje.Text = "Es necesario.";
        String Mensaje_Error = "";
        Boolean Validacion = true;
        if (Cmb_Empleados.SelectedIndex == 0)
        {
            Mensaje_Error = Mensaje_Error + "+ Seleccionar el Empleado para Resguardo.";
            Validacion = false;
        }
        if (!Validacion)
        {
            Lbl_Mensaje_Error.Text = HttpUtility.HtmlDecode(Mensaje_Error);
            Div_Contenedor_Msj_Error.Visible = true;
        }
        return Validacion;
    }


    #endregion

    #endregion

    #region Grids

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Resguardantes_PageIndexChanging
    ///DESCRIPCIÓN: Maneja el Cambio de Pagina de la Resguardantes
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 24/Enero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Resguardantes_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            if (Session["Dt_Resguardantes"] != null)
            {
                DataTable Tabla = (DataTable)Session["Dt_Resguardantes"];
                Llenar_Grid_Resguardantes(e.NewPageIndex, Tabla);
                Grid_Resguardantes.SelectedIndex = (-1);
            }
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }


    #endregion

    #region Eventos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Nuevo_Click
    ///DESCRIPCIÓN: Prepara y da de Alta un bien con uno o mas resguardantes.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 24/Enero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Btn_Nuevo.AlternateText.Equals("Nuevo"))
            {
                Configuracion_Formulario(false);
                Limpiar_Catalogo_Generales();
                Session.Remove("Dt_Resguardantes");
                Cmb_Estatus.SelectedIndex = 1;
            }
            else
            {
                if (Validar_Componentes_Generales())
                {
                    Cls_Ope_Pat_Com_Bienes_Caja_Chica_Negocio Bien = new Cls_Ope_Pat_Com_Bienes_Caja_Chica_Negocio();
                    Bien.P_Nombre = Txt_Nombre.Text.Trim();
                    Bien.P_Dependencia_ID = Cmb_Dependencias.SelectedItem.Value;
                    Bien.P_Numero_Inventario = Txt_Numero_Inventario.Text.Trim();
                    Bien.P_Cantidad = Convert.ToInt32(Txt_Cantidad.Text.Trim());
                    Bien.P_Material_ID = Cmb_Material.SelectedItem.Value.Trim();
                    Bien.P_Color_ID = Cmb_Color.SelectedItem.Value.Trim();
                    Bien.P_Marca_ID = Cmb_Marca.SelectedItem.Value.Trim();
                    Bien.P_Modelo_ID = Cmb_Modelo.SelectedItem.Value.Trim();
                    Bien.P_Costo = Convert.ToDouble(Txt_Costo.Text.Trim());
                    Bien.P_Fecha_Adquisicion = Convert.ToDateTime(Txt_Fecha_Adquisicion.Text.Trim());
                    Bien.P_Estado = Cmb_Estado.SelectedItem.Value;
                    Bien.P_Estatus = Cmb_Estatus.SelectedItem.Value;
                    Bien.P_Comentarios = Txt_Comentarios_Generales.Text.Trim();
                    if (AFU_Archivo.HasFile) { Bien.P_Archivo = AFU_Archivo.FileName; }
                    Bien.P_Resguardantes = (Session["Dt_Resguardantes"] != null) ? (DataTable)Session["Dt_Resguardantes"] : new DataTable();
                    Bien.P_Usuario_Nombre = Cls_Sessiones.Nombre_Empleado;
                    Bien.P_Usuario_ID = Cls_Sessiones.Empleado_ID;
                    Bien.Alta_Bien_Caja_Chica();
                    if (AFU_Archivo.HasFile)
                    {
                        String Ruta = Server.MapPath("../../" + Ope_Pat_Archivos_Bienes.Campo_Ruta_Fisica_Archivos + "/CAJA_CHICA/" + Bien.P_Bien_ID);
                        if (!Directory.Exists(Ruta))
                        {
                            Directory.CreateDirectory(Ruta);
                        }
                        String Archivo = Ruta + "/" + Bien.P_Archivo;
                        AFU_Archivo.SaveAs(Archivo);
                    }
                    Limpiar_Catalogo_Generales();
                    Session.Remove("Dt_Resguardantes");
                    Configuracion_Formulario(true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Caja Chica", "alert('Alta Exitosa');", true);
                }
            }
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN: Cancela la operación que esta en proceso (Alta o Actualizar) o Sale
    ///             del Formulario.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 24/Enero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Btn_Salir.AlternateText.Equals("Salir"))
            {
                Session.Remove("Dt_Resguardantes");
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
            }
            else
            {
                Grid_Resguardantes.Enabled = true;
                Configuracion_Formulario(true);
                Limpiar_Catalogo_Generales();
                Session.Remove("Dt_Resguardantes");
                Btn_Salir.AlternateText = "Salir";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
            }
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cmb_Dependencias_SelectedIndexChanged
    ///DESCRIPCIÓN: Maneja el evento de cambio de Selección del Combo de Dependencias
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 24/Enero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Cmb_Dependencias_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Session.Remove("Dt_Resguardantes");
            Grid_Resguardantes.DataSource = new DataTable();
            Grid_Resguardantes.DataBind();
            if (Cmb_Dependencias.SelectedIndex > 0)
            {
                Cls_Ope_Pat_Com_Bienes_Caja_Chica_Negocio Combo = new Cls_Ope_Pat_Com_Bienes_Caja_Chica_Negocio();
                Combo.P_Tipo_DataTable = "EMPLEADOS";
                Combo.P_Dependencia_ID = Cmb_Dependencias.SelectedItem.Value.Trim();
                DataTable Tabla = Combo.Consultar_DataTable();
                Llenar_Combo_Empleados(Tabla);
            }
            else
            {
                DataTable Tabla = new DataTable();
                Tabla.Columns.Add("EMPLEADO_ID", Type.GetType("System.String"));
                Tabla.Columns.Add("NOMBRE", Type.GetType("System.String"));
                Llenar_Combo_Empleados(Tabla);
            }
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }


    #region Resguardos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Agregar_Resguardante_Click
    ///DESCRIPCIÓN: Agrega una nuevo Empleado Resguardante para este Bien.
    ///             (No aun en la Base de Datos)
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 24/Enero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Agregar_Resguardante_Click(object sender, ImageClickEventArgs e)
    {
        if (Validar_Componentes_Resguardos())
        {
            DataTable Tabla = (DataTable)Grid_Resguardantes.DataSource;
            if (Tabla == null)
            {
                if (Session["Dt_Resguardantes"] == null)
                {
                    Tabla = new DataTable("Resguardos");
                    Tabla.Columns.Add("BIEN_RESGUARDO_ID", Type.GetType("System.String"));
                    Tabla.Columns.Add("EMPLEADO_ID", Type.GetType("System.String"));
                    Tabla.Columns.Add("NOMBRE_EMPLEADO", Type.GetType("System.String"));
                    Tabla.Columns.Add("COMENTARIOS", Type.GetType("System.String"));
                }
                else
                {
                    Tabla = (DataTable)Session["Dt_Resguardantes"];
                }
            }
            if (!Buscar_Clave_DataTable(Cmb_Empleados.SelectedItem.Value, Tabla, 1))
            {
                DataRow Fila = Tabla.NewRow();
                Fila["BIEN_RESGUARDO_ID"] = HttpUtility.HtmlDecode("");
                Fila["EMPLEADO_ID"] = HttpUtility.HtmlDecode(Cmb_Empleados.SelectedItem.Value);
                Fila["NOMBRE_EMPLEADO"] = HttpUtility.HtmlDecode(Cmb_Empleados.SelectedItem.Text);
                Fila["COMENTARIOS"] = HttpUtility.HtmlDecode(Txt_Comentarios.Text.Trim());
                Tabla.Rows.Add(Fila);
                Grid_Resguardantes.DataSource = Tabla;
                Session["Dt_Resguardantes"] = Tabla;
                Grid_Resguardantes.DataBind();
                Grid_Resguardantes.SelectedIndex = (-1);
                Cmb_Empleados.SelectedIndex = 0;
                Txt_Comentarios.Text = "";
            }
            else
            {
                Lbl_Ecabezado_Mensaje.Text = "El Empleado ya esta Agregado.";
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Quitar_Resguardante_Click
    ///DESCRIPCIÓN: Quita un Empleado resguardante para este bien (No en la Base de datos
    ///             aun).
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO:24/Enero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Quitar_Resguardante_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Grid_Resguardantes.Rows.Count > 0 && Grid_Resguardantes.SelectedIndex > (-1))
            {
                Int32 Registro = ((Grid_Resguardantes.PageIndex) * Grid_Resguardantes.PageSize) + (Grid_Resguardantes.SelectedIndex);
                if (Session["Dt_Resguardantes"] != null)
                {
                    DataTable Tabla = (DataTable)Session["Dt_Resguardantes"];
                    Tabla.Rows.RemoveAt(Registro);
                    Session["Dt_Resguardantes"] = Tabla;
                    Grid_Resguardantes.SelectedIndex = (-1);
                    Llenar_Grid_Resguardantes(Grid_Resguardantes.PageIndex, Tabla);
                }
            }
            else
            {
                Lbl_Ecabezado_Mensaje.Text = "Debe seleccionar el Registro que se desea Quitar.";
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    #endregion

    #endregion

    #region (Control Acceso Pagina)
    /// *****************************************************************************************************************************
    /// NOMBRE: Configuracion_Acceso
    /// 
    /// DESCRIPCIÓN: Habilita las operaciones que podrá realizar el usuario en la página.
    /// 
    /// PARÁMETROS: No Áplica.
    /// USUARIO CREÓ: Juan Alberto Hernández Negrete.
    /// FECHA CREÓ: 23/Mayo/2011 10:43 a.m.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *****************************************************************************************************************************
    protected void Configuracion_Acceso(String URL_Pagina)
    {
        List<ImageButton> Botones = new List<ImageButton>();//Variable que almacenara una lista de los botones de la página.
        DataRow[] Dr_Menus = null;//Variable que guardara los menus consultados.

        try
        {
            //Agregamos los botones a la lista de botones de la página.
            Botones.Add(Btn_Nuevo);

            if (!String.IsNullOrEmpty(Request.QueryString["PAGINA"]))
            {
                if (Es_Numero(Request.QueryString["PAGINA"].Trim()))
                {
                    //Consultamos el menu de la página.
                    Dr_Menus = Cls_Sessiones.Menu_Control_Acceso.Select("MENU_ID=" + Request.QueryString["PAGINA"]);

                    if (Dr_Menus.Length > 0)
                    {
                        //Validamos que el menu consultado corresponda a la página a validar.
                        if (Dr_Menus[0][Apl_Cat_Menus.Campo_URL_Link].ToString().Contains(URL_Pagina))
                        {
                            Cls_Util.Configuracion_Acceso_Sistema_SIAS_AlternateText(Botones, Dr_Menus[0]);//Habilitamos la configuracón de los botones.
                        }
                        else
                        {
                            Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                        }
                    }
                    else
                    {
                        Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                    }
                }
                else
                {
                    Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                }
            }
            else
            {
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al habilitar la configuración de accesos a la página. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: IsNumeric
    /// DESCRIPCION : Evalua que la cadena pasada como parametro sea un Numerica.
    /// PARÁMETROS: Cadena.- El dato a evaluar si es numerico.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 29/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private Boolean Es_Numero(String Cadena)
    {
        Boolean Resultado = true;
        Char[] Array = Cadena.ToCharArray();
        try
        {
            for (int index = 0; index < Array.Length; index++)
            {
                if (!Char.IsDigit(Array[index])) return false;
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al Validar si es un dato numerico. Error [" + Ex.Message + "]");
        }
        return Resultado;
    }
    #endregion
}