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
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using System.Xml.Linq;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using Presidencia.Control_Patrimonial_Operacion_Bienes_Muebles.Negocio;
using Presidencia.Control_Patrimonial_Catalogo_Donadores.Negocio;
using Presidencia.Almacen_Resguardos.Negocio;
using System.IO;
using Presidencia.Reportes;
using System.Collections.Generic;
using Presidencia.Catalogo_Compras_Productos.Negocio;
using Presidencia.Catalogo_Compras_Proveedores.Negocio;
using Presidencia.Control_Patrimonial_Catalogo_Procedencias.Negocio;
using Presidencia.Empleados.Negocios;
using Presidencia.Control_Patrimonial_Operacion_Vehiculos.Negocio;
using Presidencia.Catalogo_Compras_Marcas.Negocio;
using Presidencia.Control_Patrimonial_Catalogo_Tipos_Vehiculo.Negocio;
using Presidencia.Control_Patrimonial_Catalogo_Colores.Negocio;
using Presidencia.Control_Patrimonial_Catalogo_Clasificaciones.Negocio;
using Presidencia.Control_Patrimonial_Catalogo_Clases_Activos.Negocio;

public partial class paginas_predial_Frm_Ope_Pat_Com_Alta_Bienes_Muebles : System.Web.UI.Page
{
    #region Variables Internas

        Cls_Alm_Com_Resguardos_Negocio Consulta_Resguardos_Negocio = new Cls_Alm_Com_Resguardos_Negocio();
        Cls_Ope_Pat_Com_Bienes_Muebles_Negocio Combo = new Cls_Ope_Pat_Com_Bienes_Muebles_Negocio();
        String Fecha_Adquisicion = "";

    #endregion
    #region Page_Load

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Page_Load
        ///DESCRIPCIÓN: Metodo que se carga cada que ocurre un PostBack de la Página
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 29/Noviembre/2010 
        ///MODIFICO: Salvador Hernández Ramírez
        ///FECHA_MODIFICO  29/Diciembre/2010 
        ///CAUSA_MODIFICACIÓN Cambio el diseño de la página
        ///*******************************************************************************  
        protected void Page_Load(object sender, EventArgs e) {
            Div_Contenedor_Msj_Error.Visible = false;
            Div_MPE_Donadores_Mensaje_Error.Visible = false;
            Pnl_Donador.Style.Remove("display");
            Pnl_Donador.Style.Add("display", "none");
            
            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");
            if (!IsPostBack)
            {
                Lbl_Numero_Inventario_Anterior.Visible = false;
                Txt_Numero_Inventario_Anterior.Visible = false;
                Llenar_Combos();
                Llenar_Combo_Procedencias();
                Llenar_Combos_MPE_Busqueda_Bienes();
                Llenar_Grid_Productos(0);
                Llenar_Grid_Proveedores(0);
                String Producto_ID = null;
                String No_Requisicion = null;

                if (Request.QueryString["No_Requisicion"] != null) {
                    No_Requisicion = HttpUtility.HtmlDecode(Request.QueryString["No_Requisicion"]).Trim();
                    Session["No_Requisicion_BM"] = No_Requisicion;
                }

                if (Request.QueryString["Producto_ID"] != null) {
                    Producto_ID = HttpUtility.HtmlDecode(Request.QueryString["Producto_ID"]).Trim();
                }

                if (Request.QueryString["Fecha_Adquisicion"] != null) {
                    Fecha_Adquisicion = HttpUtility.HtmlDecode(Request.QueryString["Fecha_Adquisicion"]).Trim();
                }

                if ((Producto_ID != null) & (No_Requisicion != null)) // Si hay requisicion
                {
                    DataSet Data_Set_Datos_Producto = new DataSet();
                    Session["Fecha_Adquisicion_BM"] = Fecha_Adquisicion;
                    Data_Set_Datos_Producto = Consulta_Resguardos_Negocio.Consulta_Datos_Producto(Producto_ID, No_Requisicion);
                    Llenar_Datos_Producto(Data_Set_Datos_Producto);
                    Componenetes_Otra_Procedencia(false);
                    Div_Datos_Generales.Visible = true;
                    Txt_Dependencia.Visible = true;
                    //Configuracion_Formulario(false);
                }
                else // Si no hay requisicion
                {
                    Div_Datos_Generales.Visible = false;
                    Componenetes_Otra_Procedencia(true);
                    Txt_Dependencia.Visible = false;
                    Cmb_Dependencias.Visible = true;
                    Llenar_Combo_Dependencias();
                    Configuracion_Formulario(false);
                }
                Div_Resguardos.Visible = false;
                Div_Producto_Bien_Mueble_Padre.Visible = false;
                Div_Vehiculo_Parent.Visible = false;
            }

            //Configuracion_Acceso("Frm_Ope_Pat_Com_Alta_Bienes_Muebles.aspx");
        }

    #endregion

    #region Metodos


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Limpiar_Campos_Vehiculo_Parent
    ///DESCRIPCIÓN          : Limpia los campos de la capa de Vehiculo.
    ///PARAMETROS           :
    ///CREO                 : Francisco Antonio Gallardo Castañeda
    ///FECHA_CREO           : 04/Noviembre/2011 
    ///MODIFICO             :
    ///FECHA_MODIFICO       :
    ///CAUSA_MODIFICACIÓN   :
    ///*******************************************************************************
    private void Limpiar_Campos_Vehiculo_Parent()
    {
        Hdf_Bien_Padre_ID.Value = "";
        Txt_Vehiculo_Nombre.Text = "";
        Txt_Vehiculo_No_Inventario.Text = "";
        Txt_Vehiculo_Numero_Serie.Text = "";
        Txt_Vehiculo_Marca.Text = "";
        Txt_Vehiculo_Tipo.Text = "";
        Txt_Vehiculo_Color.Text = "";
        Txt_Vehiculo_Modelo.Text = "";
        Txt_Vehiculo_Numero_Economico.Text = "";
        Txt_Vehiculo_Placas.Text = "";
        Grid_Resguardantes.DataSource = new DataTable();
        Grid_Resguardantes.DataBind();
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Componenetes_Otra_Procedencia
    ///DESCRIPCIÓN: Metodo que se carga de ocultar o mostrar los componentes del panel
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 29/Noviembre/2010 
    ///MODIFICO: Salvador Hernández Ramírez
    ///FECHA_MODIFICO  03-Febrero-11 
    ///CAUSA_MODIFICACIÓN Cambio el diseño de la página
    ///*******************************************************************************  
    public void Componenetes_Otra_Procedencia(Boolean Estatus)
    {
        Div_Generales_Otra_Procedencia.Visible = Estatus;
        Lbl_Producto.Visible = Estatus;
        Txt_Nombre_Producto_Donado.Visible = Estatus;
        Lbl_Fecha_Adquisicion.Visible = Estatus;
        Txt_Fecha_Adquisicion.Visible = Estatus;
        Btn_Fecha_Adquisicion.Visible = Estatus;
        Txt_Modelo.Visible = Estatus;
        Txt_Garantia.Visible = Estatus;
        Lbl_Modelo.Visible = Estatus;
        Cmb_Marca.Visible = Estatus;
        Lbl_Marca.Visible = Estatus;

        if (Estatus)
        {
            Llenar_Combos_Otra_Procedencia();
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Listado_Bienes
    ///DESCRIPCIÓN: Se llenan el Grid de Bienes del Modal de Busqueda dependiendo de 
    ///             los filtros pasados.
    ///PROPIEDADES:     
    ///             1. Pagina.  Pagina en donde aparecerá el Grid.
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 01/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Grid_Listado_Bienes(Int32 Pagina){
        try{
                Grid_Listado_Bienes.Columns[1].Visible = true;
                Cls_Ope_Pat_Com_Bienes_Muebles_Negocio Bienes = new Cls_Ope_Pat_Com_Bienes_Muebles_Negocio();
                Bienes.P_Tipo_DataTable = "BIENES";
                if (Session["FILTRO_BUSQUEDA"] != null) {
                    Bienes.P_Tipo_Filtro_Busqueda = Session["FILTRO_BUSQUEDA"].ToString();
                    if (Session["FILTRO_BUSQUEDA"].ToString().Trim().Equals("DATOS_GENERALES")) {
                        if (Txt_Busqueda_Inventario_Anterior    .Text.Trim().Length > 0)
                        {
                            Bienes.P_Numero_Inventario_Anterior = Txt_Busqueda_Inventario_Anterior.Text.Trim();
                        }
                        if (Txt_Busqueda_Inventario_SIAS.Text.Trim().Length > 0)
                        {
                            Bienes.P_Numero_Inventario = Txt_Busqueda_Inventario_SIAS.Text.Trim();
                        }
                        if (Txt_Busqueda_Producto.Text.Trim().Length > 0) {
                            Bienes.P_Nombre_Producto = Txt_Busqueda_Producto.Text.Trim();
                        }
                        if (Cmb_Busqueda_Dependencias.SelectedIndex > 0) {
                            Bienes.P_Dependencia_ID = Cmb_Busqueda_Dependencias.SelectedItem.Value;
                        }
                        if (Txt_Busqueda_Modelo.Text.Trim().Length > 0) {
                            Bienes.P_Modelo = Txt_Busqueda_Modelo.Text.Trim();
                        }
                        if (Cmb_Busqueda_Marca.SelectedIndex > 0) {
                            Bienes.P_Marca_ID = Cmb_Busqueda_Marca.SelectedItem.Value;
                        }
                        if (Cmb_Busqueda_Estatus.SelectedIndex > 0) {
                            Bienes.P_Estatus = Cmb_Busqueda_Estatus.SelectedItem.Value;
                        }
                        if (Txt_Busqueda_Factura.Text.Trim().Length > 0) {
                            Bienes.P_Factura = Txt_Busqueda_Factura.Text.Trim();
                        }
                        if (Txt_Busqueda_Numero_Serie.Text.Trim().Length > 0) {
                            Bienes.P_Numero_Serie = Txt_Busqueda_Numero_Serie.Text.Trim();
                        }
                    } else if (Session["FILTRO_BUSQUEDA"].ToString().Trim().Equals("RESGUARDANTES")) {
                        if (Txt_Busqueda_RFC_Resguardante.Text.Trim().Length > 0) {
                            Bienes.P_RFC_Resguardante = Txt_Busqueda_RFC_Resguardante.Text.Trim();
                        }
                        if (Cmb_Busqueda_Resguardantes_Dependencias.SelectedIndex > 0) {
                            Bienes.P_Dependencia_ID = Cmb_Busqueda_Resguardantes_Dependencias.SelectedItem.Value.Trim();
                        }
                        if (Cmb_Busqueda_Nombre_Resguardante.SelectedIndex > 0) {
                            Bienes.P_Resguardante_ID = Cmb_Busqueda_Nombre_Resguardante.SelectedItem.Value.Trim();
                        }                    
                    }
                }
                Grid_Listado_Bienes.DataSource = Bienes.Consultar_DataTable();
                Grid_Listado_Bienes.PageIndex = Pagina;
                Grid_Listado_Bienes.DataBind();
                Grid_Listado_Bienes.Columns[1].Visible = false;
                MPE_Busqueda_Bien_Mueble.Show();
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Empleados
    ///DESCRIPCIÓN: Llena el combo de Empleados.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 30/Noviembre/2010 
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
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Procedencias
    ///DESCRIPCIÓN: Llena el combo de Procedencias.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 07/Julio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Combo_Procedencias() {
        Cls_Cat_Pat_Com_Procedencias_Negocio Procedencia_Negocio = new Cls_Cat_Pat_Com_Procedencias_Negocio();
        Procedencia_Negocio.P_Estatus = "VIGENTE";
        Procedencia_Negocio.P_Tipo_DataTable = "PROCEDENCIAS";
        Cmb_Procedencia.DataSource = Procedencia_Negocio.Consultar_DataTable();
        Cmb_Procedencia.DataTextField = "NOMBRE";
        Cmb_Procedencia.DataValueField = "PROCEDENCIA_ID";
        Cmb_Procedencia.DataBind();
        Cmb_Procedencia.Items.Insert(0, new ListItem("<-- SELECCIONE -->", ""));
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Dependencias
    ///DESCRIPCIÓN: Llena el combo de Dependencias.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 15/Marzo/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Combo_Dependencias()
    {
        //SE LLENA EL COMBO DE DEPENDENCIAS
        Combo.P_Tipo_DataTable = "DEPENDENCIAS";
        DataTable Dependencias = Combo.Consultar_DataTable();
        Cmb_Dependencias.DataSource = Dependencias;
        Cmb_Dependencias.DataValueField = "DEPENDENCIA_ID";
        Cmb_Dependencias.DataTextField = "NOMBRE";
        Cmb_Dependencias.DataBind();
        Cmb_Dependencias.Items.Insert(0, new ListItem("<- SELECCIONE ->", ""));
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN:  Llenar_Combos_Otra_Procedencia()
    ///DESCRIPCIÓN: Se llenan los DropDowList  Modelos y Marcas.
    ///PROPIEDADES:     
    ///CREO: Salvador Hérnandez Ramírez.
    ///FECHA_CREO: 03/Febrero/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Combos_Otra_Procedencia()
    {
        try
        {
            Cls_Ope_Pat_Com_Bienes_Muebles_Negocio Combos = new Cls_Ope_Pat_Com_Bienes_Muebles_Negocio();

            //SE LLENA EL COMBO DE MARCAS
            Combos.P_Tipo_DataTable = "MARCAS";
            Cmb_Marca_Parent.DataSource = Combos.Consultar_DataTable();
            Cmb_Marca_Parent.DataValueField = "MARCA_ID";
            Cmb_Marca_Parent.DataTextField = "NOMBRE";
            Cmb_Marca_Parent.DataBind();
            Cmb_Marca_Parent.Items.Insert(0, new ListItem("<- SELECCIONE ->", ""));

            //SE LLENA EL COMBO DE MATERIALES
            Combos.P_Tipo_DataTable = "MATERIALES";
            Cmb_Material_Parent.DataSource = Combos.Consultar_DataTable();
            Cmb_Material_Parent.DataValueField = "MATERIAL_ID";
            Cmb_Material_Parent.DataTextField = "DESCRIPCION";
            Cmb_Material_Parent.DataBind();
            Cmb_Material_Parent.Items.Insert(0, new ListItem("<- SELECCIONE ->", ""));

            //SE LLENA EL COMBO DE COLORES
            Combos.P_Tipo_DataTable = "COLORES";
            Cmb_Color_Parent.DataSource = Combos.Consultar_DataTable();
            Cmb_Color_Parent.DataValueField = "COLOR_ID";
            Cmb_Color_Parent.DataTextField = "DESCRIPCION";
            Cmb_Color_Parent.DataBind();
            Cmb_Color_Parent.Items.Insert(0, new ListItem("<- SELECCIONE ->", ""));

        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combos
    ///DESCRIPCIÓN: Se llenan los Combos Generales Independientes.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 29/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Combos()
    {
        try
        {
            Cls_Ope_Pat_Com_Bienes_Muebles_Negocio Combos = new Cls_Ope_Pat_Com_Bienes_Muebles_Negocio();

            //SE LLENA EL COMBO DE MATERIALES
            Combos.P_Tipo_DataTable = "MATERIALES";
            DataTable Materiales = Combos.Consultar_DataTable();
            DataRow Fila_Material = Materiales.NewRow();
            Fila_Material["MATERIAL_ID"] = "SELECCIONE";
            Fila_Material["DESCRIPCION"] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
            Materiales.Rows.InsertAt(Fila_Material, 0);
            Cmb_Materiales.DataSource = Materiales;
            Cmb_Materiales.DataValueField = "MATERIAL_ID";
            Cmb_Materiales.DataTextField = "DESCRIPCION";
            Cmb_Materiales.DataBind();

            //SE LLENA EL COMBO DE COLORES
            Combos.P_Tipo_DataTable = "COLORES";
            DataTable Colores = Combos.Consultar_DataTable();
            DataRow Fila_Color = Colores.NewRow();
            Fila_Color["COLOR_ID"] = "SELECCIONE";
            Fila_Color["DESCRIPCION"] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
            Colores.Rows.InsertAt(Fila_Color, 0);
            Cmb_Colores.DataSource = Colores;
            Cmb_Colores.DataValueField = "COLOR_ID";
            Cmb_Colores.DataTextField = "DESCRIPCION";
            Cmb_Colores.DataBind();

            //SE LLENA EL COMBO DE MARCAS DE LAS BUSQUEDAS
            Combos.P_Tipo_DataTable = "MARCAS";
            Cmb_Marca.DataSource = Combos.Consultar_DataTable(); ;
            Cmb_Marca.DataTextField = "NOMBRE";
            Cmb_Marca.DataValueField = "MARCA_ID";
            Cmb_Marca.DataBind();
            Cmb_Marca.Items.Insert(0, new ListItem("<-- SELECCIONE -->", ""));

            Combo.P_Tipo_DataTable = "ZONAS";
            DataTable Zonas = Combo.Consultar_DataTable();
            Zonas.DefaultView.Sort = "DESCRIPCION";
            Cmb_Zonas.DataSource = Zonas;
            Cmb_Zonas.DataTextField = "DESCRIPCION";
            Cmb_Zonas.DataValueField = "ZONA_ID";
            Cmb_Zonas.DataBind();
            Cmb_Zonas.Items.Insert(0, new ListItem("<--SELECCIONE-->", ""));

            Cls_Cat_Pat_Com_Clases_Activo_Negocio CA_Negocio = new Cls_Cat_Pat_Com_Clases_Activo_Negocio();
            CA_Negocio.P_Estatus = "VIGENTE";
            CA_Negocio.P_Tipo_DataTable = "CLASES_ACTIVOS";
            Cmb_Clase_Activo.DataSource = CA_Negocio.Consultar_DataTable();
            Cmb_Clase_Activo.DataValueField = "CLASE_ACTIVO_ID";
            Cmb_Clase_Activo.DataTextField = "CLAVE_DESCRIPCION";
            Cmb_Clase_Activo.DataBind();
            Cmb_Clase_Activo.Items.Insert(0, new ListItem("<- SELECCIONE ->", ""));

            Cls_Cat_Pat_Com_Clasificaciones_Negocio Clasificaciones_Negocio = new Cls_Cat_Pat_Com_Clasificaciones_Negocio();
            Clasificaciones_Negocio.P_Estatus = "VIGENTE";
            Clasificaciones_Negocio.P_Tipo_DataTable = "CLASIFICACIONES";
            Cmb_Tipo_Activo.DataSource = Clasificaciones_Negocio.Consultar_DataTable();
            Cmb_Tipo_Activo.DataValueField = "CLASIFICACION_ID";
            Cmb_Tipo_Activo.DataTextField = "CLAVE_DESCRIPCION";
            Cmb_Tipo_Activo.DataBind();
            Cmb_Tipo_Activo.Items.Insert(0, new ListItem("<- SELECCIONE ->", ""));

        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combos_MPE_Busqueda_Bienes
    ///DESCRIPCIÓN: Se llenan los Combos del Modal de Busqueda.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 23/Marzo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Combos_MPE_Busqueda_Bienes()
    {
        try
        {
            Cls_Ope_Pat_Com_Bienes_Muebles_Negocio Combos = new Cls_Ope_Pat_Com_Bienes_Muebles_Negocio();

            //SE LLENA EL COMBO DE DEPENDENCIAS DE LAS BUSQUEDAS
            Combos.P_Tipo_DataTable = "DEPENDENCIAS";
            DataTable Dependencias = Combos.Consultar_DataTable();
            DataRow Fila_Dependencia = Dependencias.NewRow();
            Fila_Dependencia["DEPENDENCIA_ID"] = "TODAS";
            Fila_Dependencia["NOMBRE"] = HttpUtility.HtmlDecode("&lt;TODAS&gt;");
            Dependencias.Rows.InsertAt(Fila_Dependencia, 0);
            Cmb_Busqueda_Dependencias.DataSource = Dependencias;
            Cmb_Busqueda_Dependencias.DataValueField = "DEPENDENCIA_ID";
            Cmb_Busqueda_Dependencias.DataTextField = "NOMBRE";
            Cmb_Busqueda_Dependencias.DataBind();
            Cmb_Busqueda_Resguardantes_Dependencias.DataSource = Dependencias;
            Cmb_Busqueda_Resguardantes_Dependencias.DataValueField = "DEPENDENCIA_ID";
            Cmb_Busqueda_Resguardantes_Dependencias.DataTextField = "NOMBRE";
            Cmb_Busqueda_Resguardantes_Dependencias.DataBind();
            Cmb_Busqueda_Dependencia.DataSource = Dependencias;
            Cmb_Busqueda_Dependencia.DataValueField = "DEPENDENCIA_ID";
            Cmb_Busqueda_Dependencia.DataTextField = "NOMBRE";
            Cmb_Busqueda_Dependencia.DataBind();


            //SE LLENA EL COMBO DE MARCAS DE LAS BUSQUEDAS
            Combos.P_Tipo_DataTable = "MARCAS";
            DataTable Marcas = Combos.Consultar_DataTable();
            DataRow Fila_Marca = Marcas.NewRow();
            Fila_Marca["MARCA_ID"] = "TODAS";
            Fila_Marca["NOMBRE"] = HttpUtility.HtmlDecode("&lt;TODAS&gt;");
            Marcas.Rows.InsertAt(Fila_Marca, 0);
            Cmb_Busqueda_Marca.DataSource = Marcas;
            Cmb_Busqueda_Marca.DataTextField = "NOMBRE";
            Cmb_Busqueda_Marca.DataValueField = "MARCA_ID";
            Cmb_Busqueda_Marca.DataBind();
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
    ///FECHA_CREO: 29/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Configuracion_Formulario(Boolean Estatus)
    {
        if (Estatus)
        {
            Btn_Nuevo.AlternateText = "Guardar";
            Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
            Btn_Salir.AlternateText = "Cancelar";
            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
            Btn_Generar_Reporte.Visible = false;
        }
        else
        {
            Btn_Nuevo.AlternateText = "Nuevo";
            Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
            Btn_Salir.AlternateText = "Salir";
            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
            Btn_Generar_Reporte.Visible = true;
            
        }
        Txt_Numero_Inventario_Anterior.Enabled = Estatus;
        Btn_Buscar_RFC_Donador.Visible = Estatus;
        Btn_Agregar_Donador.Visible = Estatus;
        Btn_Limpiar_Donador.Visible = Estatus;
        Txt_RFC_Donador.Enabled = Estatus;
        Cmb_Procedencia.Enabled = Estatus;
        Cmb_Tipo_Activo.Enabled = Estatus;
        Cmb_Clase_Activo.Enabled = Estatus;
        Cmb_Operacion.Enabled = Estatus;
        Txt_Nombre_Donador.Enabled = Estatus;
        Txt_Dependencia.Enabled = Estatus;
        Cmb_Dependencias.Enabled = Estatus;
        Cmb_Zonas.Enabled = Estatus;
        Cmb_Materiales.Enabled = Estatus;
        Cmb_Colores.Enabled = Estatus;
        Txt_Numero_Inventario.Enabled = false;
        Txt_Factura.Enabled = Estatus;
        Txt_Costo.Enabled = Estatus;
        Txt_Numero_Serie.Enabled = Estatus;
        Cmb_Estado.Enabled = Estatus;
        Txt_Observaciones.Enabled = Estatus;
        Cmb_Empleados.Enabled = Estatus;
        Txt_Cometarios.Enabled = Estatus;
        Cmb_Asignacion_Secundaria.Enabled = Estatus;

        Btn_Lanzar_Mpe_Productos.Visible = Estatus;
        Btn_Lanzar_Mpe_Proveedores.Visible = Estatus;
        Btn_Fecha_Adquisicion.Enabled = Estatus;
        Txt_Modelo.Enabled = Estatus;
        Txt_Garantia.Enabled = Estatus;
        Cmb_Marca.Enabled = Estatus;
        Btn_Agregar_Resguardante.Visible = Estatus;
        Btn_Quitar_Resguardante.Visible = Estatus;
        AFU_Archivo.Enabled = Estatus;
        Btn_Busqueda_Avanzada_Resguardante.Visible = Estatus;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Limpiar_Catalogo
    ///DESCRIPCIÓN: Limpia los controles Generales del Formulario
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 29/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Limpiar_Catalogo_Generales()
    {
        Txt_Producto_ID.Text = "";
        Txt_Nombre_Producto.Text = "";
        Txt_Marca_Producto.Text = "";
        Txt_Marca_Producto.Text = "";
        Txt_Proveedor_Producto.Text = "";
        Txt_Clave_Producto.Text = "";
        Txt_Costo_Producto.Text = "";
        Hdf_Producto_ID.Value = "";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Limpiar_Detalles
    ///DESCRIPCIÓN: Limpia los controles de detalles del Formulario
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 30/Noviembre/2010 
    ///MODIFICO: 15/ Diciembre/2010
    ///FECHA_MODIFICO Salvador Hernández Ramírez
    ///CAUSA_MODIFICACIÓN: cambiaron los componentes, por lo tanto no se van a limpiar todos los detalles
    ///*******************************************************************************
    private void Limpiar_Detalles()
    {
        Hdf_Bien_Mueble_ID.Value = "";
        Cmb_Clase_Activo.SelectedIndex = 0;
        Cmb_Tipo_Activo.SelectedIndex = 0;
        Hdf_Donador_ID.Value = "";
        Txt_RFC_Donador.Text = "";
        Txt_Nombre_Donador.Text = "";
        Cmb_Procedencia.SelectedIndex = 0;
        Cmb_Dependencias.SelectedIndex = 0;
        Cmb_Dependencias_SelectedIndexChanged(Cmb_Dependencias, null);
        Cmb_Marca.SelectedIndex = 0;
        Txt_Modelo.Text = "";
        Txt_Nombre_Proveedor.Text = "";
        Hdf_Proveedor_ID.Value = "";
        Txt_Garantia.Text = "";
        Txt_Nombre_Producto_Donado.Text = "";
        Txt_Fecha_Adquisicion.Text = "";
        Cmb_Materiales.SelectedIndex = 0;
        Cmb_Zonas.SelectedIndex = 0;
        Cmb_Colores.SelectedIndex = 0;
        Txt_Numero_Inventario.Text = "";
        Txt_Numero_Inventario_Anterior.Text = "";
        Txt_Factura.Text = "";
        Txt_Costo.Text = "";
        Txt_Numero_Serie.Text = "";
        Cmb_Estatus.SelectedIndex = 0;
        Cmb_Estado.SelectedIndex = 0;
        Txt_Observaciones.Text = "";
        Cmb_Empleados.SelectedIndex = 0;
        Txt_Cometarios.Text = "";
        Grid_Resguardantes.SelectedIndex = -1;
        Grid_Resguardantes.DataSource = new DataTable();
        Grid_Resguardantes.DataBind();
        Remover_Sesiones_Control_AsyncFileUpload(AFU_Archivo.ClientID);
        Cmb_Asignacion_Secundaria.SelectedIndex = 0;
        Cmb_Asignacion_Secundaria_SelectedIndexChanged(Cmb_Asignacion_Secundaria, null);
        Div_Resguardos.Visible = false;
        Div_Producto_Bien_Mueble_Padre.Visible = false;
        Hdf_Bien_Padre_ID.Value = "";
        Limpiar_Parent();
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
    ///FECHA_CREO: 29/Noviembre/2010 
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

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_DataSet_Resguardos_Bienes()
    ///DESCRIPCIÓN: Llena el dataSet "Data_Set_Resguardos_Bienes" con las personas a las que se les asigno el
    ///bien mueble y sus detalles, para que con estos datos se genere el reporte.
    ///PARAMETROS:  
    ///CREO: Salvador Hernández Ramírez
    ///FECHA_CREO: 17/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Llenar_DataSet_Resguardos_Bienes(Cls_Ope_Pat_Com_Bienes_Muebles_Negocio Bien_Id)
    {
        String Formato = "PDF";
        String Resguardantes = "";
        String RFC = "";

        Consulta_Resguardos_Negocio = new Cls_Alm_Com_Resguardos_Negocio();
        DataTable Dt_Resguardos_Bienes;

        if (Session["OPERACION_MB"] != null)
            Consulta_Resguardos_Negocio.P_Operacion = Session["OPERACION_MB"].ToString().Trim();
        Dt_Resguardos_Bienes = Consulta_Resguardos_Negocio.Consulta_Resguardos_Bienes(Bien_Id);

        if (Session["OPERACION_MB"].ToString().Trim() == "RESGUARDO")
            Dt_Resguardos_Bienes.Rows[0].SetField("OPERACION", "CONTROL DE RESGUARDOS DE BIENES MUEBLES");
        else if (Session["OPERACION_MB"].ToString().Trim() == "RECIBO")
            Dt_Resguardos_Bienes.Rows[0].SetField("OPERACION", "CONTROL DE RECIBOS DE BIENES MUEBLES");

        if ((string.IsNullOrEmpty(Dt_Resguardos_Bienes.Rows[0]["MARCA"].ToString())) | (Dt_Resguardos_Bienes.Rows[0]["MARCA"].ToString().Trim() == ""))
            Dt_Resguardos_Bienes.Rows[0].SetField("MARCA", "INDISTINTA");

        if (string.IsNullOrEmpty(Dt_Resguardos_Bienes.Rows[0]["MODELO"].ToString()))
            Dt_Resguardos_Bienes.Rows[0].SetField("MODELO", "INDISTINTO");

        Ds_Alm_Com_Resguardos_Bienes Ds_Consulta_Resguardos_Bienes = new Ds_Alm_Com_Resguardos_Bienes();
        Generar_Reporte(Dt_Resguardos_Bienes, Ds_Consulta_Resguardos_Bienes, Formato);
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Generar_Reporte
    ///DESCRIPCIÓN:          Caraga el data set fisoco con el cual se genera el Reporte especificado
    ///PARAMETROS:           1.-Data_Set_Consulta_DB.- Contiene la informacion de la consulta a la base de datos
    ///                      2.-Ds_Reporte, Objeto que contiene la instancia del Data set fisico del Reporte a generar
    ///                      3.-Nombre_Reporte, contiene el nombre del Reporte a mostrar en pantalla
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           15/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Generar_Reporte(DataTable Dt_Consulta_DB, DataSet Ds_Reporte, String Formato)
    {

        String Ruta_Reporte_Crystal = "";
        String Nombre_Reporte_Generar = "";
        DataRow Renglon;

        try
        {
            Renglon = Dt_Consulta_DB.Rows[0];
            String Cantidad = Dt_Consulta_DB.Rows[0]["CANTIDAD"].ToString();
            String Costo = Dt_Consulta_DB.Rows[0]["COSTO_UNITARIO"].ToString();
            Double Resultado = (Convert.ToDouble(Cantidad)) * (Convert.ToDouble(Costo));
            Ds_Reporte.Tables[1].ImportRow(Renglon);
            Ds_Reporte.Tables[1].Rows[0].SetField("COSTO_TOTAL", Resultado);

            for (int Cont_Elementos = 0; Cont_Elementos < Dt_Consulta_DB.Rows.Count; Cont_Elementos++)
            {
                Renglon = Dt_Consulta_DB.Rows[Cont_Elementos];
                Ds_Reporte.Tables[0].ImportRow(Renglon);
                String Nombre_E = Dt_Consulta_DB.Rows[Cont_Elementos]["NOMBRE_E"].ToString();
                String Apellido_Paterno_E = Dt_Consulta_DB.Rows[Cont_Elementos]["APELLIDO_PATERNO_E"].ToString();
                String Apellido_Materno_E = Dt_Consulta_DB.Rows[Cont_Elementos]["APELLIDO_MATERNO_E"].ToString();
                String RFC_E = Dt_Consulta_DB.Rows[Cont_Elementos]["RFC_E"].ToString();
                String Resguardante = Nombre_E + " " + Apellido_Paterno_E + " " + Apellido_Materno_E + " " + "(" + RFC_E + ")";
                Ds_Reporte.Tables[0].Rows[Cont_Elementos].SetField("RESGUARDANTES", Resguardante);
            }

            // Ruta donde se encuentra el reporte Crystal
            Ruta_Reporte_Crystal = "../Rpt/Almacen/Rpt_Alm_Com_Resguardos_Bienes_Almacen.rpt";

            // Se crea el nombre del reporte
            String Nombre_Reporte = "Rpt_Resguardo_Bienes_" + Cls_Sessiones.No_Empleado + "_" + Convert.ToString(DateTime.Now.ToString("yyyy'-'MM'-'dd'_t'HH'-'mm'-'ss"));

            // Se da el nombre del reporte que se va generar
            if (Formato == "PDF")
                Nombre_Reporte_Generar = Nombre_Reporte + ".pdf";  // Es el nombre del reporte PDF que se va a generar
            else if (Formato == "Excel")
                Nombre_Reporte_Generar = Nombre_Reporte + ".xls";  // Es el nombre del repote en Excel que se va a generar

            Cls_Reportes Reportes = new Cls_Reportes();
            Reportes.Generar_Reporte(ref Ds_Reporte, Ruta_Reporte_Crystal, Nombre_Reporte_Generar, Formato);
            Mostrar_Reporte(Nombre_Reporte_Generar, Formato);
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al llenar el DataSet. Error: [" + Ex.Message + "]");
        }
    }

    /// *************************************************************************************
    /// NOMBRE:              Mostrar_Reporte
    /// DESCRIPCIÓN:         Muestra el reporte en pantalla.
    /// PARÁMETROS:          Nombre_Reporte_Generar.- Nombre que tiene el reporte que se mostrará en pantalla.
    ///                      Formato.- Variable que contiene el formato en el que se va a generar el reporte "PDF" O "Excel"
    /// USUARIO CREO:        Juan Alberto Hernández Negrete.
    /// FECHA CREO:          3/Mayo/2011 18:20 p.m.
    /// USUARIO MODIFICO:    Salvador Hernández Ramírez
    /// FECHA MODIFICO:      16-Mayo-2011
    /// CAUSA MODIFICACIÓN:  Se asigno la opción para que en el mismo método se muestre el reporte en excel
    /// *************************************************************************************
    protected void Mostrar_Reporte(String Nombre_Reporte_Generar, String Formato)
    {
        String Pagina = "../Paginas_Generales/Frm_Apl_Mostrar_Reportes.aspx?Reporte=";

        try
        {
            if (Formato == "PDF")
            {
                Pagina = Pagina + Nombre_Reporte_Generar;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Window_Rpt",
                "window.open('" + Pagina + "', 'Reporte','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
            }
            else if (Formato == "Excel")
            {
                String Ruta = "../../Reporte/" + Nombre_Reporte_Generar;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + Ruta + "', 'Reportes','toolbar=0,dire ctories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al mostrar el reporte. Error: [" + Ex.Message + "]");
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Limpiar_Catalogo_Donadores
    ///DESCRIPCIÓN: Limpia los controles del panel utilizado para capturar los datos del donador
    ///PROPIEDADES:     
    ///CREO: Salvador Hernández Ramírez.
    ///FECHA_CREO: 01/Febrero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Limpiar_Catalogo_Donadores()
    {
        Txt_Nombre_Donador_MPE.Text = "";
        Txt_Apellido_Paterno_Donador.Text = "";
        Txt_Apellido_Materno_Donador.Text = "";
        Txt_Direccion_Donador.Text = "";
        Txt_Ciudad_Donador.Text = "";
        Txt_Estado_Donador.Text = "";
        Txt_Telefono_Donador.Text = "";
        Txt_Celular_Donador.Text = "";
        Txt_CURP_Donador.Text = "";
        Txt_RFC_Donador_MPE.Text = "";
    }
    
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Limpiar_Parent
    ///DESCRIPCIÓN: Limpia los campos donde se muestran los detalles del Parent.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 25/Marzo/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Limpiar_Parent() {
        try
        {
            Hdf_Bien_Padre_ID.Value = "";
            Txt_Inventario_SIAS.Text = "";
            Txt_Numero_Inventario_Parent.Text = "";
            Cmb_Material_Parent.SelectedIndex = 0;
            Txt_Modelo_Parent.Text = "";
            Cmb_Marca_Parent.SelectedIndex = 0;
            Cmb_Color_Parent.SelectedIndex = 0;
            Txt_Nombre_Parent.Text = "";
            Txt_Costo_Parent.Text = "";
            Txt_Fecha_Adquisicion_Parent.Text = "";
            Cmb_Estado_Parent.SelectedIndex = 0;
            Cmb_Estatus_Parent.SelectedIndex = 0;
            Txt_Observaciones_Parent.Text = "";
        } catch (Exception Ex) {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Consultar_Empleados
    ///DESCRIPCIÓN:          Metodo utilizado para consultar y llenar el combo con los empleados que pertenecen a la dependencia
    ///PROPIEDADES:     
    ///                      1.  Dependencia_ID. el identificador de la dependencia en base a la que se va hacer la consulta
    ///              
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO: 02/Febrero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    public void Consultar_Empleados(String Dependencia_ID)
    {
        try
        {
            Session.Remove("Dt_Resguardantes");
            Grid_Resguardantes.DataSource = new DataTable();
            Grid_Resguardantes.DataBind();

            Cls_Ope_Pat_Com_Bienes_Muebles_Negocio Combo = new Cls_Ope_Pat_Com_Bienes_Muebles_Negocio();
            Combo.P_Tipo_DataTable = "EMPLEADOS";
            Combo.P_Dependencia_ID = Dependencia_ID;
            DataTable Tabla = Combo.Consultar_DataTable();
            Llenar_Combo_Empleados(Tabla);
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Empleados_Busqueda
    ///DESCRIPCIÓN: Llena el combo de Empleados del Modal de Busqueda.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 01/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Combo_Empleados_Busqueda(DataTable Tabla)
    {
        try
        {
            DataRow Fila_Empleado = Tabla.NewRow();
            Fila_Empleado["EMPLEADO_ID"] = "SELECCIONE";
            Fila_Empleado["NOMBRE"] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
            Tabla.Rows.InsertAt(Fila_Empleado, 0);
            Cmb_Busqueda_Nombre_Resguardante.DataSource = Tabla;
            Cmb_Busqueda_Nombre_Resguardante.DataValueField = "EMPLEADO_ID";
            Cmb_Busqueda_Nombre_Resguardante.DataTextField = "NOMBRE";
            Cmb_Busqueda_Nombre_Resguardante.DataBind();
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    #region Grids

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Datos_Producto
    ///DESCRIPCIÓN: Llena el Grid de Productos.
    ///PROPIEDADES:     
    ///            1. Data_Set_Datos_Producto, es el dataSet que contiene la información de los productos
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 29/Noviembre/2010 
    ///MODIFICO: Salvador Hernández Ramírez
    ///FECHA_MODIFICO: 29/Diciembre/2010
    ///CAUSA_MODIFICACIÓN: La asignación de los datos del producto y la dependencia, ya no se cuando se selecciona del datagrid, 
    ///el producto, ahora se realiza a través de una consulta a la BD
    ///*******************************************************************************
    private void Llenar_Datos_Producto(DataSet Data_Set_Datos_Producto)
    {
        Txt_Producto_ID.Text = Data_Set_Datos_Producto.Tables[0].Rows[0]["PRODUCTO_ID"].ToString();
        Txt_Clave_Producto.Text = Data_Set_Datos_Producto.Tables[0].Rows[0]["CLAVE"].ToString();
        Txt_Nombre_Producto.Text = Data_Set_Datos_Producto.Tables[0].Rows[0]["NOMBRE_PRODUCTO"].ToString();
        Txt_Marca_Producto.Text = Data_Set_Datos_Producto.Tables[0].Rows[0]["NOMBRE_MARCA"].ToString();
        Txt_Modelo_Producto.Text = Data_Set_Datos_Producto.Tables[0].Rows[0]["NOMBRE_MODELO"].ToString();
        Txt_Proveedor_Producto.Text = Data_Set_Datos_Producto.Tables[0].Rows[0]["NOMBRE_PROVEEDOR"].ToString();
        Txt_Costo_Producto.Text = ("$" + " " + Data_Set_Datos_Producto.Tables[0].Rows[0]["COSTO"].ToString());
        Txt_Dependencia.Text = Data_Set_Datos_Producto.Tables[0].Rows[0]["NOMBRE_DEPENDENCIA"].ToString();
        Hdf_Dependencia_ID.Value = Data_Set_Datos_Producto.Tables[0].Rows[0]["DEPENDENCIA_ID"].ToString();
        String Depencencia_Id = Data_Set_Datos_Producto.Tables[0].Rows[0]["DEPENDENCIA_ID"].ToString();
        Consultar_Empleados(Depencencia_Id);
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Resguardantes
    ///DESCRIPCIÓN: Llena la tabla de Resguardantes
    ///PROPIEDADES:     
    ///             1.  Pagina. Pagina en la cual se mostrará el Grid_VIew
    ///             2.  tabla.  Tabla que se va a cargar en el Grid.    
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 29/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Grid_Resguardantes(Int32 Pagina, DataTable Tabla)
    {
        Grid_Resguardantes.Columns[1].Visible = true;
        Grid_Resguardantes.Columns[2].Visible = true;
        Grid_Resguardantes.DataSource = Tabla;
        Grid_Resguardantes.PageIndex = Pagina;
        Session["Dt_Resguardantes_BM"] = Tabla;
        Grid_Resguardantes.DataBind();
        Grid_Resguardantes.Columns[1].Visible = false;
        Grid_Resguardantes.Columns[2].Visible = false;
    }

    #endregion

    #region Validaciones

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Componentes_Generales
    ///DESCRIPCIÓN: Hace una validacion de que haya datos en los componentes antes de hacer
    ///             una operación de la pestaña de Generales.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 29/Noviembre/2010 
    ///MODIFICO: Salvador Hernández Ramírez
    ///FECHA_MODIFICO 03/Febrero/2011 
    ///CAUSA_MODIFICACIÓN  Se agregarón las validaciones de los componentes que el usuario debe agregar cuando se resguarda un bien donado
    ///*******************************************************************************
    private bool Validar_Componentes_Generales()
    {
        Lbl_Ecabezado_Mensaje.Text = "Es necesario.";
        String Mensaje_Error = "";
        Boolean Validacion = true;
        if (Cmb_Estatus.SelectedIndex == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Debe seleccionar una opción del Combo de Estatus.";
            Validacion = false;
        }
        if (Cmb_Materiales.SelectedIndex == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Debe seleccionar una opción del Combo de Materiales.";
            Validacion = false;
        }
        if (Cmb_Colores.SelectedIndex == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Debe seleccionar una opción del Combo de Colores.";
            Validacion = false;
        }
        if (Txt_Factura.Text.Trim().Length == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Introducir la Factura del Bien.";
            Validacion = false;
        }
        if (Txt_Costo.Text.Trim().Length == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Introducir el Costo Actual del Bien.";
            Validacion = false;
        }
        //if (Txt_Nombre_Proveedor.Text.Trim().Length == 0)
        //{
        //    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
        //    Mensaje_Error = Mensaje_Error + "+ Introducir el proveedor del Bien.";
        //    Validacion = false;
        //}
        if (Txt_Numero_Serie.Text.Trim().Length == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Introducir el Número de Serie del Bien.";
            Validacion = false;
        }
        if (Cmb_Estado.SelectedIndex == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Debe seleccionar una opción del Combo de Estado.";
            Validacion = false;
        }
        if (Txt_Producto_ID.Text.Trim() == "")  {

            if (Cmb_Procedencia.SelectedIndex == 0) {
                if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                Mensaje_Error = Mensaje_Error + "+ Debe seleccionar una opción del Combo Procedencia.";
                Validacion = false;
            }
            if (Cmb_Dependencias.SelectedIndex == 0) {
                if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                Mensaje_Error = Mensaje_Error + "+ Debe seleccionar una opción del Combo Unidad Responsable.";
                Validacion = false;
            }
            //if (Cmb_Modelo.SelectedIndex == 0) {
            //    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            //    Mensaje_Error = Mensaje_Error + "+ Debe seleccionar una opción del Combo Modelo.";
            //    Validacion = false;
            //}
            //if (Cmb_Marca.SelectedIndex == 0) {
            //    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            //    Mensaje_Error = Mensaje_Error + "+ Debe seleccionar una opción del Combo Marca.";
            //    Validacion = false;
            //}
            if (Hdf_Producto_ID.Value.Trim().Length == 0) {
                if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                Mensaje_Error = Mensaje_Error + "+ Seleccionar el Producto.";
                Validacion = false;
            }
            if (Txt_Fecha_Adquisicion.Text.Trim().Length == 0) {
                if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                Mensaje_Error = Mensaje_Error + "+ Introducir la Fecha de Facturación.";
                Validacion = false;
            }
        }
        //if (Txt_Numero_Inventario_Anterior.Text.Trim().Length == 0) {
        //        if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
        //        Mensaje_Error = Mensaje_Error + "+ Introducir el Inventario Anterior.";
        //        Validacion = false;
        //}
        if (Cmb_Asignacion_Secundaria.SelectedItem.Equals("EXISTENTE")) {
            if (Hdf_Bien_Padre_ID.Value != null && Hdf_Bien_Padre_ID.Value.Trim().Length > 0) {
                if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br/>"; }
                Mensaje_Error = Mensaje_Error + "+ Seleccione el bien al que depende.";
                Validacion = false;
            }
        }
        if (!Validacion) {
            Lbl_Mensaje_Error.Text = HttpUtility.HtmlDecode(Mensaje_Error);
            Div_Contenedor_Msj_Error.Visible = true;
        }
        return Validacion;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Componentes_Resguardos
    ///DESCRIPCIÓN: Hace una validacion de que haya datos en los componentes antes de hacer
    ///             una operación de la pestaña de Resguardos.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 29/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private bool Validar_Componentes_Resguardos()
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

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Catalogo_Donador
    ///DESCRIPCIÓN: Hace una validacion de que haya datos en los componentes antes de hacer
    ///             una operación para dar de alta un donador.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 22/Enero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private bool Validar_Catalogo_Donador()
    {
        Lbl_MPE_Donadores_Cabecera_Error.Text = "Es necesario.";
        String Mensaje_Error = "";
        Boolean Validacion = true;
        if (Txt_Nombre_Donador_MPE.Text.Trim().Length == 0)
        {
            Mensaje_Error = Mensaje_Error + "+ Introducir el Nombre del Donador.";
            Validacion = false;
        }
        if (Txt_RFC_Donador_MPE.Text.Trim().Length == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br/>"; }
            Mensaje_Error = Mensaje_Error + "+ Introducir el RFC del Donador.";
            Validacion = false;
        }
        if (!Validacion)
        {
            Lbl_MPE_Donadores_Mensaje_Error.Text = HttpUtility.HtmlDecode(Mensaje_Error);
            Div_MPE_Donadores_Mensaje_Error.Visible = true;
            MPE_Donadores.Show();
        }
        return Validacion;
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Productos
    ///DESCRIPCIÓN: Llena el Grid de los Productos para seleccionarlo.
    ///PARAMETROS: Pagina. Pagina del Grid que se mostrará.     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 04/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Grid_Productos(Int32 Pagina) {
        Grid_Listado_Productos.SelectedIndex = (-1);
        Grid_Listado_Productos.Columns[1].Visible = true;
        Cls_Cat_Com_Productos_Negocio Productos_Negocio = new Cls_Cat_Com_Productos_Negocio();
        if (Txt_Nombre_Producto_Buscar.Text.Trim() != "") {
            Productos_Negocio.P_Nombre = Txt_Nombre_Producto_Buscar.Text.Trim();
        }
        Productos_Negocio.P_Estatus = "ACTIVO";
        //Productos_Negocio.P_Tipo = "BIEN_MUEBLE";
        DataTable Dt_Productos = Productos_Negocio.Consulta_Datos_Producto();
        Dt_Productos.Columns[Cat_Com_Productos.Campo_Producto_ID].ColumnName = "PRODUCTO_ID";
        Dt_Productos.Columns[Cat_Com_Productos.Campo_Clave].ColumnName = "CLAVE_PRODUCTO";
        Dt_Productos.Columns[Cat_Com_Productos.Campo_Nombre].ColumnName = "NOMBRE_PRODUCTO";
        Dt_Productos.Columns[Cat_Com_Productos.Campo_Descripcion].ColumnName = "DESCRIPCION_PRODUCTO";
        Grid_Listado_Productos.DataSource = Dt_Productos;
        Grid_Listado_Productos.PageIndex = Pagina;
        Grid_Listado_Productos.DataBind();
        Grid_Listado_Productos.Columns[1].Visible = false;
    }



    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Proveedores
    ///DESCRIPCIÓN:      Llena el Grid de los Proveedores para que el usuario lo seleccione
    ///PARAMETROS:       Pagina. Pagina del Grid que se mostrará.     
    ///CREO:             Salvador Hernández Ramírez
    ///FECHA_CREO:       08/Agosto/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Grid_Proveedores(Int32 Pagina)
    {
        Grid_Listado_Proveedores.SelectedIndex = (-1);
        Grid_Listado_Proveedores.Columns[1].Visible = true;
        Cls_Cat_Com_Proveedores_Negocio Proveedores_Negocio = new Cls_Cat_Com_Proveedores_Negocio();
        if (Txt_Nombre_Proveedor_Buscar.Text.Trim() != "")
        {
            Proveedores_Negocio.P_Busqueda = Txt_Nombre_Proveedor_Buscar.Text.Trim();
        }
        DataTable Dt_Proveedores = Proveedores_Negocio.Consulta_Datos_Proveedores();
        Dt_Proveedores.Columns[Cat_Com_Proveedores.Campo_Proveedor_ID].ColumnName = "PROVEEDOR_ID";
        Dt_Proveedores.Columns[Cat_Com_Proveedores.Campo_Nombre].ColumnName = "NOMBRE";
        Dt_Proveedores.Columns[Cat_Com_Proveedores.Campo_RFC].ColumnName = "RFC";
        Dt_Proveedores.Columns[Cat_Com_Proveedores.Campo_Compañia].ColumnName = "COMPANIA";
        Grid_Listado_Proveedores.DataSource = Dt_Proveedores;
        Grid_Listado_Proveedores.PageIndex = Pagina;
        Grid_Listado_Proveedores.DataBind();
        Grid_Listado_Proveedores.Columns[1].Visible = false;
    }

    #endregion

        #region "Busqueda Resguardantes"

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Busqueda_Empleados_Resguardo
        ///DESCRIPCIÓN: Llena el Grid con los empleados que cumplan el filtro
        ///PROPIEDADES:     
        ///CREO:                 
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 24/Octubre/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Llenar_Grid_Busqueda_Empleados_Resguardo() {
            Grid_Busqueda_Empleados_Resguardo.SelectedIndex = (-1);
            Grid_Busqueda_Empleados_Resguardo.Columns[1].Visible = true;
            Cls_Ope_Pat_Com_Bienes_Muebles_Negocio Negocio = new Cls_Ope_Pat_Com_Bienes_Muebles_Negocio();
            if (Txt_Busqueda_No_Empleado.Text.Trim().Length > 0) { Negocio.P_No_Empleado_Resguardante = Txt_Busqueda_No_Empleado.Text.Trim(); }
            if (Txt_Busqueda_RFC.Text.Trim().Length > 0) { Negocio.P_RFC_Resguardante = Txt_Busqueda_RFC.Text.Trim(); }
            if (Txt_Busqueda_Nombre_Empleado.Text.Trim().Length > 0) { Negocio.P_Nombre_Resguardante = Txt_Busqueda_Nombre_Empleado.Text.Trim(); }
            if (Cmb_Busqueda_Dependencia.SelectedIndex > 0) { Negocio.P_Dependencia_ID = Cmb_Busqueda_Dependencia.SelectedItem.Value; }
            Grid_Busqueda_Empleados_Resguardo.DataSource = Negocio.Consultar_Empleados_Resguardos();
            Grid_Busqueda_Empleados_Resguardo.DataBind();
            Grid_Busqueda_Empleados_Resguardo.Columns[1].Visible = false;
        }

        #endregion
        

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Llenar_Grid_Listado_Vehiculos
        ///DESCRIPCIÓN          : Llena el Grid de los Vehiculos.
        ///PARAMETROS           : 
        ///CREO                 : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO           : 18/Noviembre/2011 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        private void Llenar_Grid_Listado_Bienes_Vehiculos(Int32 Pagina) { 
           try {
                Grid_Listado_Busqueda_Vehiculo.Columns[1].Visible = true;
                Cls_Ope_Pat_Com_Vehiculos_Negocio Vehiculos = new Cls_Ope_Pat_Com_Vehiculos_Negocio();
                Vehiculos.P_Tipo_DataTable = "VEHICULOS";
                if (Session["FILTRO_BUSQUEDA"] != null) {
                    Vehiculos.P_Tipo_Filtro_Busqueda = Session["FILTRO_BUSQUEDA"].ToString();
                    if (Session["FILTRO_BUSQUEDA"].ToString().Trim().Equals("DATOS_GENERALES")) {
                        if (Txt_Busqueda_Vehiculo_Numero_Inventario.Text.Trim().Length > 0) { Vehiculos.P_Numero_Inventario = Convert.ToInt64(Txt_Busqueda_Vehiculo_Numero_Inventario.Text.Trim()); }
                        if (Txt_Busqueda_Vehiculo_Numero_Economico.Text.Trim().Length > 0) { Vehiculos.P_Numero_Economico_ = Txt_Busqueda_Vehiculo_Numero_Economico.Text.Trim(); }
                        if (Txt_Busqueda_Vehiculo_Anio_Fabricacion.Text.Trim().Length > 0) { Vehiculos.P_Anio_Fabricacion = Convert.ToInt32(Txt_Busqueda_Vehiculo_Anio_Fabricacion.Text.Trim()); }
                        Vehiculos.P_Modelo_ID = Txt_Busqueda_Vehiculo_Modelo.Text.Trim();
                        if (Cmb_Busqueda_Vehiculo_Marca.SelectedIndex > 0) {
                            Vehiculos.P_Marca_ID = Cmb_Busqueda_Vehiculo_Marca.SelectedItem.Value.Trim();
                        }
                        if (Cmb_Busqueda_Vehiculo_Tipo_Vehiculo.SelectedIndex > 0) {
                            Vehiculos.P_Tipo_Vehiculo_ID = Cmb_Busqueda_Vehiculo_Tipo_Vehiculo.SelectedItem.Value.Trim();
                        }
                        if (Cmb_Busqueda_Vehiculo_Tipo_Combustible.SelectedIndex > 0) {
                            Vehiculos.P_Tipo_Combustible_ID = Cmb_Busqueda_Vehiculo_Tipo_Combustible.SelectedItem.Value.Trim();
                        }
                        if (Cmb_Busqueda_Vehiculo_Color.SelectedIndex > 0) {
                            Vehiculos.P_Color_ID = Cmb_Busqueda_Vehiculo_Color.SelectedItem.Value.Trim();
                        }
                        if (Cmb_Busqueda_Vehiculo_Zonas.SelectedIndex > 0) {
                            Vehiculos.P_Zona_ID = Cmb_Busqueda_Vehiculo_Zonas.SelectedItem.Value.Trim();
                        }
                        if (Cmb_Busqueda_Vehiculo_Estatus.SelectedIndex > 0) {
                            Vehiculos.P_Estatus = Cmb_Busqueda_Vehiculo_Estatus.SelectedItem.Value.Trim();
                        }
                        if (Cmb_Busqueda_Vehiculo_Dependencias.SelectedIndex > 0) {
                            Vehiculos.P_Dependencia_ID = Cmb_Busqueda_Vehiculo_Dependencias.SelectedItem.Value.Trim();
                        }
                    } else if (Session["FILTRO_BUSQUEDA"].ToString().Trim().Equals("RESGUARDANTES")) {
                        Vehiculos.P_RFC_Resguardante = Txt_Busqueda_Vehiculo_RFC_Resguardante.Text.Trim();
                        Vehiculos.P_RFC_Resguardante = Txt_Busqueda_Vehiculo_No_Empleado.Text.Trim();
                        if (Cmb_Busqueda_Vehiculo_Resguardantes_Dependencias.SelectedIndex > 0) {
                            Vehiculos.P_Dependencia_ID = Cmb_Busqueda_Vehiculo_Resguardantes_Dependencias.SelectedItem.Value.Trim();
                        }
                        if (Cmb_Busqueda_Vehiculo_Nombre_Resguardante.SelectedIndex > 0) {
                            Vehiculos.P_Resguardante_ID = Cmb_Busqueda_Vehiculo_Nombre_Resguardante.SelectedItem.Value.Trim();
                        }
                    }
                }
                Grid_Listado_Busqueda_Vehiculo.DataSource = Vehiculos.Consultar_DataTable();
                Grid_Listado_Busqueda_Vehiculo.PageIndex = Pagina;
                Grid_Listado_Busqueda_Vehiculo.DataBind();
                Grid_Listado_Busqueda_Vehiculo.Columns[1].Visible = false;
                MPE_Busqueda_Vehiculo.Show();
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Llenar_Combo_Empleados
        ///DESCRIPCIÓN          : Llena el Combo con los empleados de una dependencia.
        ///PARAMETROS           : 
        ///CREO                 : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO           : 22/Noviembre/2011 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        private void Llenar_Combo_Empleados(String Dependencia_ID, ref DropDownList Combo_Empleados)
        {
            Combo_Empleados.Items.Clear();
            if (Dependencia_ID != null && Dependencia_ID.Trim().Length > 0)
            {
                Cls_Ope_Pat_Com_Bienes_Muebles_Negocio BSI_Negocio = new Cls_Ope_Pat_Com_Bienes_Muebles_Negocio();
                BSI_Negocio.P_Tipo_DataTable = "EMPLEADOS";
                BSI_Negocio.P_Dependencia_ID = Dependencia_ID.Trim();
                DataTable Dt_Datos = BSI_Negocio.Consultar_DataTable();
                Combo_Empleados.DataSource = Dt_Datos;
                Combo_Empleados.DataValueField = "EMPLEADO_ID";
                Combo_Empleados.DataTextField = "EMPLEADO";
                Combo_Empleados.DataBind();
            }
            Combo_Empleados.Items.Insert(0, new ListItem("< TODOS >", ""));
        }

    #endregion

    #region Grid

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Resguardantes_PageIndexChanging
    ///DESCRIPCIÓN: Maneja la paginación del GridView de los reguardantesa
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 17/Marzo/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Resguardantes_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            DataTable Tabla = new DataTable();
            if (Session["Dt_Resguardantes_BM"] != null)
            {
                Tabla = (DataTable)Session["Dt_Resguardantes_BM"];
            }
            Llenar_Grid_Resguardantes(e.NewPageIndex, Tabla);
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Listado_Bienes_PageIndexChanging
    ///DESCRIPCIÓN: Maneja la paginación del GridView de Bienes del Modal de Busqueda
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 01/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Listado_Bienes_PageIndexChanging(object sender, GridViewPageEventArgs e) {
        try {
            Grid_Listado_Bienes.SelectedIndex = (-1);
            Llenar_Grid_Listado_Bienes(e.NewPageIndex);
        } catch (Exception Ex) {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Listado_Bienes_SelectedIndexChanged
    ///DESCRIPCIÓN: Maneja el evento de cambio de Seleccion del GridView de Bienes del
    ///             Modal de Busqueda.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 02/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Listado_Bienes_SelectedIndexChanged(object sender, EventArgs e) {
        try {
            if (Grid_Listado_Bienes.SelectedIndex > (-1)) {
                Limpiar_Parent();
                String Bien_ID = Grid_Listado_Bienes.SelectedRow.Cells[1].Text.Trim();
                Cls_Ope_Pat_Com_Bienes_Muebles_Negocio Bien_Mueble = new Cls_Ope_Pat_Com_Bienes_Muebles_Negocio();
                Bien_Mueble.P_Bien_Mueble_ID = Bien_ID;
                Bien_Mueble = Bien_Mueble.Consultar_Detalles_Bien_Mueble();
                Hdf_Bien_Padre_ID.Value = Bien_Mueble.P_Bien_Mueble_ID;
                Txt_Numero_Inventario_Parent.Text = Bien_Mueble.P_Numero_Inventario_Anterior;
                Txt_Inventario_SIAS.Text = Bien_Mueble.P_Numero_Inventario;
                Txt_Nombre_Parent.Text = Bien_Mueble.P_Nombre_Producto.ToString();
                Txt_Modelo_Parent.Text = Bien_Mueble.P_Modelo;
                Cmb_Marca_Parent.SelectedIndex = Cmb_Marca_Parent.Items.IndexOf(Cmb_Marca_Parent.Items.FindByValue(Bien_Mueble.P_Marca_ID));
                Cmb_Material_Parent.SelectedIndex = Cmb_Material_Parent.Items.IndexOf(Cmb_Material_Parent.Items.FindByValue(Bien_Mueble.P_Material_ID));
                Cmb_Color_Parent.SelectedIndex = Cmb_Color_Parent.Items.IndexOf(Cmb_Color_Parent.Items.FindByValue(Bien_Mueble.P_Color_ID));
                Txt_Costo_Parent.Text = "$ " + Bien_Mueble.P_Costo_Actual.ToString();
                Txt_Fecha_Adquisicion_Parent.Text = (String.Format("{0:dd'/'MMMMMMMMMMMMMMM'/'yyyy}", Convert.ToDateTime(Bien_Mueble.P_Fecha_Adquisicion))).ToUpper();
                Cmb_Estado_Parent.SelectedIndex = Cmb_Estado_Parent.Items.IndexOf(Cmb_Estado_Parent.Items.FindByValue(Bien_Mueble.P_Estado));
                Cmb_Estatus_Parent.SelectedIndex = Cmb_Estatus_Parent.Items.IndexOf(Cmb_Estatus_Parent.Items.FindByValue(Bien_Mueble.P_Estatus));
                Cmb_Dependencias.SelectedIndex = Cmb_Dependencias.Items.IndexOf(Cmb_Dependencias.Items.FindByValue(Bien_Mueble.P_Dependencia_ID));
                Cmb_Dependencias_SelectedIndexChanged(Cmb_Dependencias, null);
                Txt_Observaciones_Parent.Text = Bien_Mueble.P_Observaciones;
                Session["Dt_Resguardantes_BM"] = Bien_Mueble.P_Resguardantes;
                Llenar_Grid_Resguardantes(0, Bien_Mueble.P_Resguardantes);
                MPE_Busqueda_Bien_Mueble.Hide();
                System.Threading.Thread.Sleep(500);
            }
        } catch (Exception Ex) {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Listado_Productos_PageIndexChanging
    ///DESCRIPCIÓN: Maneja la paginación del GridView de los Productos
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO:04/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Listado_Productos_PageIndexChanging(object sender, GridViewPageEventArgs e) {
        try {
            Llenar_Grid_Productos(e.NewPageIndex);
        } catch (Exception Ex) {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Listado_Productos_SelectedIndexChanged
    ///DESCRIPCIÓN: Maneja el evento de cambio de Seleccion del GridView de Productos del
    ///             Modal de Busqueda.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 04/Julio/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Listado_Productos_SelectedIndexChanged(object sender, EventArgs e) {
        try {
            if (Grid_Listado_Productos.SelectedIndex > (-1)) {
                String Producto_ID = Grid_Listado_Productos.SelectedRow.Cells[1].Text.Trim();
                Cls_Cat_Com_Productos_Negocio Producto_Negocio = new Cls_Cat_Com_Productos_Negocio();
                Producto_Negocio.P_Producto_ID = Producto_ID;
                Producto_Negocio.P_Estatus = "ACTIVO";
                DataTable Dt_Producto_Seleccionado = Producto_Negocio.Consulta_Datos_Producto();
                if(Dt_Producto_Seleccionado != null && Dt_Producto_Seleccionado.Rows.Count>0){
                    Hdf_Producto_ID.Value = Dt_Producto_Seleccionado.Rows[0][Cat_Com_Productos.Campo_Producto_ID].ToString().Trim();
                    Txt_Nombre_Producto_Donado.Text = Dt_Producto_Seleccionado.Rows[0][Cat_Com_Productos.Campo_Nombre].ToString().Trim();
                    Mpe_Productos_Cabecera.Hide();
                }
                System.Threading.Thread.Sleep(500);
                Grid_Listado_Productos.SelectedIndex = (-1);
            }
        } catch (Exception Ex) {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Listado_Productos_SelectedIndexChanged
    ///DESCRIPCIÓN: Maneja el evento de cambio de Seleccion del GridView de Productos del
    ///             Modal de Busqueda.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 04/Julio/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Listado_Proveedores_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Grid_Listado_Proveedores.SelectedIndex > (-1))
            {
                String Proveedor_ID = Grid_Listado_Proveedores.SelectedRow.Cells[1].Text.Trim();
       
                Cls_Cat_Com_Proveedores_Negocio Proveedor_Negocio = new Cls_Cat_Com_Proveedores_Negocio();
                Proveedor_Negocio.P_Proveedor_ID = Proveedor_ID;
                DataTable Dt_Proveedor_Seleccionado = Proveedor_Negocio.Consulta_Datos_Proveedores();
                if (Dt_Proveedor_Seleccionado != null && Dt_Proveedor_Seleccionado.Rows.Count > 0)
                {
                    Hdf_Proveedor_ID.Value = Proveedor_ID.Trim();

                        Txt_Nombre_Proveedor.Text = Dt_Proveedor_Seleccionado.Rows[0][Cat_Com_Proveedores.Campo_Nombre].ToString().Trim();

                    Mpe_Proveedores_Cabecera.Hide();
                }
                System.Threading.Thread.Sleep(500);
                Grid_Listado_Proveedores.SelectedIndex = (-1);
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
    ///NOMBRE DE LA FUNCIÓN: Grid_Listado_Proveedores_PageIndexChanging
    ///DESCRIPCIÓN: Maneja la paginación del GridView de Proveedores del Modal de Busqueda
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 23/Septiembre/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Listado_Proveedores_PageIndexChanging(object sender, GridViewPageEventArgs e) {
        try {
            Grid_Listado_Proveedores.SelectedIndex = (-1);
            Llenar_Grid_Proveedores(e.NewPageIndex);
        } catch (Exception Ex) {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }
    
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Busqueda_Empleados_Resguardo_PageIndexChanging
        ///DESCRIPCIÓN: Maneja el evento de cambio de Página del GridView de Busqueda
        ///             de empleados.
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 24/Octubre/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Busqueda_Empleados_Resguardo_PageIndexChanging(object sender, GridViewPageEventArgs e) {
            try {
                Grid_Busqueda_Empleados_Resguardo.PageIndex = e.NewPageIndex;
                Llenar_Grid_Busqueda_Empleados_Resguardo();
                MPE_Resguardante.Show();
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Busqueda_Empleados_Resguardo_SelectedIndexChanged
        ///DESCRIPCIÓN: Maneja el evento de cambio de Selección del GridView de Busqueda
        ///             de empleados.
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 24/Octubre/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Busqueda_Empleados_Resguardo_SelectedIndexChanged(object sender, EventArgs e) { 
            try {
                if (Grid_Busqueda_Empleados_Resguardo.SelectedIndex > (-1)) {
                    String Empleado_Seleccionado_ID = Grid_Busqueda_Empleados_Resguardo.SelectedRow.Cells[1].Text.Trim();
                    Cls_Cat_Empleados_Negocios Empleado_Negocio = new Cls_Cat_Empleados_Negocios();
                    Empleado_Negocio.P_Empleado_ID = Empleado_Seleccionado_ID.Trim();
                    DataTable Dt_Datos_Empleado = Empleado_Negocio.Consulta_Empleados_General();
                    String Dependencia_ID = (!String.IsNullOrEmpty(Dt_Datos_Empleado.Rows[0][Cat_Dependencias.Campo_Dependencia_ID].ToString())) ? Dt_Datos_Empleado.Rows[0][Cat_Dependencias.Campo_Dependencia_ID].ToString() : null;
                    Int32 Index_Combo = (-1);
                    if (Dependencia_ID != null && Dependencia_ID.Trim().Length > 0) {
                        Index_Combo = Cmb_Dependencias.Items.IndexOf(Cmb_Dependencias.Items.FindByValue(Dependencia_ID));
                        if (Index_Combo > (-1)) {
                            if (Index_Combo == Cmb_Dependencias.SelectedIndex) {
                                Cmb_Empleados.SelectedIndex = Cmb_Empleados.Items.IndexOf(Cmb_Empleados.Items.FindByValue(Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Empleado_ID].ToString()));
                            } else {
                                Cmb_Dependencias.SelectedIndex = Cmb_Dependencias.Items.IndexOf(Cmb_Dependencias.Items.FindByValue(Dependencia_ID));
                                Consultar_Empleados(Dependencia_ID);
                                Cmb_Empleados.SelectedIndex = Cmb_Empleados.Items.IndexOf(Cmb_Empleados.Items.FindByValue(Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Empleado_ID].ToString()));
                            }
                        }
                    }
                    MPE_Resguardante.Hide();
                }
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Listado_Productos_PageIndexChanging
        ///DESCRIPCIÓN: Maneja la paginación del GridView de los Vehiculos
        ///PROPIEDADES:     
        ///CREO                 : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO           : 18/Noviembre/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Listado_Busqueda_Vehiculo_PageIndexChanging(object sender, GridViewPageEventArgs e) {
            try {
                Llenar_Grid_Listado_Bienes_Vehiculos(e.NewPageIndex);
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Listado_Busqueda_Vehiculo_SelectedIndexChanged
        ///DESCRIPCIÓN: Maneja el evento de cambio de Seleccion del GridView de Vehiculos del
        ///             Modal de Busqueda.
        ///PROPIEDADES:     
        ///CREO     : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO   : 18/Noviembre/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Listado_Busqueda_Vehiculo_SelectedIndexChanged(object sender, EventArgs e) {
            try {
                Limpiar_Campos_Vehiculo_Parent();
                if (Grid_Listado_Busqueda_Vehiculo.SelectedIndex > (-1)) {
                    String Vehiculo_ID = Grid_Listado_Busqueda_Vehiculo.SelectedRow.Cells[1].Text.Trim();
                    Cls_Ope_Pat_Com_Vehiculos_Negocio Vehiculo_Negocio = new Cls_Ope_Pat_Com_Vehiculos_Negocio();
                    Vehiculo_Negocio.P_Vehiculo_ID = Vehiculo_ID;
                    Vehiculo_Negocio = Vehiculo_Negocio.Consultar_Detalles_Vehiculo();
                    Mostrar_Detalles_Vehiculo(Vehiculo_Negocio);
                    MPE_Busqueda_Vehiculo.Hide();
                    Grid_Listado_Busqueda_Vehiculo.SelectedIndex = (-1);
                }
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }
    
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Mostrar_Detalles_Vehiculo
        ///DESCRIPCIÓN          : Muestra la información de un Vehículo.
        ///PARAMETROS           : Vehiculo. Datos a Cargar.
        ///CREO                 : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO           : 11/Noviembre/2011 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        private void Mostrar_Detalles_Vehiculo(Cls_Ope_Pat_Com_Vehiculos_Negocio Vehiculo) {
            if (Vehiculo.P_Vehiculo_ID != null && Vehiculo.P_Vehiculo_ID.Trim().Length > 0) {
                Hdf_Bien_Padre_ID.Value = Vehiculo.P_Vehiculo_ID;
                Txt_Vehiculo_Nombre.Text = Vehiculo.P_Nombre_Producto;
                Cmb_Dependencias.SelectedIndex = Cmb_Dependencias.Items.IndexOf(Cmb_Dependencias.Items.FindByValue(Vehiculo.P_Dependencia_ID));
                Txt_Vehiculo_No_Inventario.Text = Vehiculo.P_Numero_Inventario.ToString();
                Txt_Vehiculo_Numero_Serie.Text = Vehiculo.P_Serie_Carroceria.Trim();
                if (Vehiculo.P_Marca_ID != null && Vehiculo.P_Marca_ID.Trim().Length > 0) {
                    Cls_Cat_Com_Marcas_Negocio Marca_Negocio = new Cls_Cat_Com_Marcas_Negocio();
                    Marca_Negocio.P_Marca_ID = Vehiculo.P_Marca_ID;
                    DataTable Dt_Marcas = Marca_Negocio.Consulta_Marcas();
                    if (Dt_Marcas != null && Dt_Marcas.Rows.Count > 0) {
                        Txt_Vehiculo_Marca.Text = Dt_Marcas.Rows[0][Cat_Com_Marcas.Campo_Nombre].ToString();
                    }
                }
                Txt_Vehiculo_Modelo.Text = Vehiculo.P_Modelo_ID;
                if (Vehiculo.P_Tipo_Vehiculo_ID != null && Vehiculo.P_Tipo_Vehiculo_ID.Trim().Length > 0) {
                    Cls_Cat_Pat_Com_Tipos_Vehiculo_Negocio Tipo_Negocio = new Cls_Cat_Pat_Com_Tipos_Vehiculo_Negocio();
                    Tipo_Negocio.P_Tipo_Vehiculo_ID = Vehiculo.P_Tipo_Vehiculo_ID;
                    Tipo_Negocio = Tipo_Negocio.Consultar_Datos_Vehiculo();
                    if (Tipo_Negocio.P_Tipo_Vehiculo_ID != null && Tipo_Negocio.P_Tipo_Vehiculo_ID.Trim().Length > 0) {
                        Txt_Vehiculo_Tipo.Text = Tipo_Negocio.P_Descripcion;
                    }
                }
                if (Vehiculo.P_Color_ID != null && Vehiculo.P_Color_ID.Trim().Length > 0) {
                    Cls_Cat_Pat_Com_Colores_Negocio Color_Negocio = new Cls_Cat_Pat_Com_Colores_Negocio();
                    Color_Negocio.P_Color_ID = Vehiculo.P_Color_ID;
                    Color_Negocio.P_Tipo_DataTable = "COLORES";
                    DataTable Dt_Colores = Color_Negocio.Consultar_DataTable();
                    if (Dt_Colores != null && Dt_Colores.Rows.Count > 0) {
                        Txt_Vehiculo_Color.Text = Dt_Colores.Rows[0][Cat_Pat_Colores.Campo_Descripcion].ToString();
                    }
                }
                Txt_Vehiculo_Numero_Economico.Text = Vehiculo.P_Numero_Economico_;
                Txt_Vehiculo_Placas.Text = Vehiculo.P_Placas;
                Vehiculo.P_Resguardantes.Columns.Add("EMPLEADO_ALMACEN_ID", Type.GetType("System.String"));
                Llenar_Grid_Resguardantes(0,Vehiculo.P_Resguardantes);
            }
        }

    #endregion

    #region Eventos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Nuevo_Click
    ///DESCRIPCIÓN: Prepara y da de Alta un Bien Mueble con uno o mas resguardantes.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 29/Noviembre/2010 
    ///MODIFICO: Salvador Hernández Ramírez
    ///FECHA_MODIFICO: 14/Diciembre/2010
    ///CAUSA_MODIFICACIÓN: Cambio el diseño de esta página, por lo tanto fue necesario quitar algunas funciones
    ///que se mandan llamar en esta funcion
    ///*******************************************************************************
    protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e) {
        String Mensaje_Error = "";
        if (Btn_Nuevo.AlternateText == "Nuevo") {
            Session.Remove("Dt_Resguardantes_BM");
            Configuracion_Formulario(true);
            Limpiar_Detalles();
            Cmb_Estatus.SelectedIndex = 1;
            Cmb_Estado.SelectedIndex = 1;
            Cmb_Asignacion_Secundaria.SelectedIndex = Cmb_Asignacion_Secundaria.Items.IndexOf(Cmb_Asignacion_Secundaria.Items.FindByValue("NINGUNA"));
            Cmb_Asignacion_Secundaria_SelectedIndexChanged(Cmb_Asignacion_Secundaria, null);
        } else if (Btn_Nuevo.AlternateText == "Guardar")  {
            try {
                if (Validar_Componentes_Generales()) {
                    if (Grid_Resguardantes.Rows.Count == 0 || Session["Dt_Resguardantes_BM"] == null) {
                        Mensaje_Error = Mensaje_Error + "Debe haber como minimo un empleado para el resguardo del Bien.";
                        Lbl_Mensaje_Error.Text = HttpUtility.HtmlDecode(Mensaje_Error);
                        Div_Contenedor_Msj_Error.Visible = true;
                    } else {
                        Cls_Ope_Pat_Com_Bienes_Muebles_Negocio Bien_Mueble = new Cls_Ope_Pat_Com_Bienes_Muebles_Negocio();
                        if (Txt_Producto_ID.Text != null && Txt_Producto_ID.Text.Trim().Length > 0) {
                            Bien_Mueble.P_Producto_ID = Txt_Producto_ID.Text.Trim();
                            if (Session["No_Requisicion_BM"] != null) {
                                Bien_Mueble.P_No_Requisicion = Session["No_Requisicion_BM"].ToString().Trim();
                            } else {
                                Bien_Mueble.P_No_Requisicion = null;
                            }
                            Bien_Mueble.P_Dependencia_ID = Hdf_Dependencia_ID.Value.Trim();
                            Bien_Mueble.P_Procedencia = "REQUISICIÓN";
                            Bien_Mueble.P_Fecha_Adquisicion = Session["Fecha_Adquisicion_BM"].ToString();
                        } else {
                            Bien_Mueble.P_Clase_Activo_ID = Cmb_Clase_Activo.SelectedItem.Value.Trim();
                            Bien_Mueble.P_Clasificacion_ID = Cmb_Tipo_Activo.SelectedItem.Value.Trim();
                            Bien_Mueble.P_Procedencia = Cmb_Procedencia.SelectedItem.Value;
                            Bien_Mueble.P_Dependencia_ID = Cmb_Dependencias.SelectedItem.Value.Trim();
                            Bien_Mueble.P_Proveedor_ID = Hdf_Proveedor_ID.Value.Trim(); // Se asigna el Proveedor
                            Bien_Mueble.P_Nombre_Producto = Txt_Nombre_Producto_Donado.Text.Trim();
                            Bien_Mueble.P_Fecha_Adquisicion_ = Convert.ToDateTime(Txt_Fecha_Adquisicion.Text.Trim());

                            if (Txt_Modelo.Text.Trim()!= "")
                                Bien_Mueble.P_Modelo = Txt_Modelo.Text.Trim();
                            else
                                Bien_Mueble.P_Modelo = "";

                            if (Txt_Garantia.Text.Trim() != "")
                                Bien_Mueble.P_Garantia = Txt_Garantia.Text.Trim();
                            else
                                Bien_Mueble.P_Garantia = "";

                            if (Cmb_Marca.SelectedIndex != 0)
                            Bien_Mueble.P_Marca_ID = Cmb_Marca.SelectedItem.Value;
                            else
                                Bien_Mueble.P_Marca_ID = "";

                            Bien_Mueble.P_Producto_ID = Hdf_Producto_ID.Value.Trim();
                            Bien_Mueble.P_Producto_Almacen = false;
                        }
                        if (Cmb_Asignacion_Secundaria.SelectedIndex>1) {
                            Bien_Mueble.P_Ascencendia = Hdf_Bien_Padre_ID.Value;
                        }
                        Bien_Mueble.P_Proveniente = Cmb_Asignacion_Secundaria.SelectedItem.Value;
                        Bien_Mueble.P_Material_ID = Cmb_Materiales.SelectedItem.Value.Trim();
                        Bien_Mueble.P_Color_ID = Cmb_Colores.SelectedItem.Value.Trim();
                        Bien_Mueble.P_Factura = Txt_Factura.Text.Trim();
                        Bien_Mueble.P_Numero_Serie = Txt_Numero_Serie.Text.Trim();
                        Bien_Mueble.P_Costo_Actual = Convert.ToDouble(Txt_Costo.Text.Trim());
                        Bien_Mueble.P_Estatus = Cmb_Estatus.SelectedItem.Value.Trim();
                        Bien_Mueble.P_Estado = Cmb_Estado.SelectedItem.Value.Trim();
                        if (AFU_Archivo.HasFile) { Bien_Mueble.P_Archivo = AFU_Archivo.FileName; }
                        if (Txt_Observaciones.Text.Trim().Length > 255) {
                            Bien_Mueble.P_Observaciones = Txt_Observaciones.Text.Trim().Substring(0, 254);
                        } else {
                            Bien_Mueble.P_Observaciones = Txt_Observaciones.Text.Trim();
                        }
                        
                        if (Cmb_Zonas.SelectedIndex > 0)
                            Bien_Mueble.P_Zona = Cmb_Zonas.SelectedItem.Value.Trim();
                        else
                            Bien_Mueble.P_Zona = null;

                        Bien_Mueble.P_Resguardantes = (DataTable)Session["Dt_Resguardantes_BM"];
                        Bien_Mueble.P_Usuario_Nombre = Cls_Sessiones.Nombre_Empleado;
                        Bien_Mueble.P_Usuario_ID = Cls_Sessiones.Empleado_ID;
                        Bien_Mueble.P_Cantidad = 1;
                        Bien_Mueble.P_Operacion = Cmb_Operacion.SelectedItem.Value.ToString().Trim(); // Se asigna el comentario que indica si es resguardo o recibo
                        Session["OPERACION_MB"] = "" + Cmb_Operacion.SelectedItem.Value.ToString().Trim();

                        Bien_Mueble.Alta_Bien_Mueble();
                        Hdf_Bien_Mueble_ID.Value = Bien_Mueble.P_Bien_Mueble_ID.Trim();
                        Txt_Numero_Inventario.Text = Bien_Mueble.P_Numero_Inventario;
                        Txt_Numero_Inventario_Anterior.Text = Bien_Mueble.P_Numero_Inventario;
                        if (AFU_Archivo.HasFile) {
                            String Ruta = Server.MapPath("../../" + Ope_Pat_Archivos_Bienes.Campo_Ruta_Fisica_Archivos + "/BIENES_MUEBLES/" + Bien_Mueble.P_Bien_Mueble_ID);
                            if (!Directory.Exists(Ruta)) {
                                Directory.CreateDirectory(Ruta);
                            }
                            String Archivo = Ruta + "/" + Bien_Mueble.P_Archivo;
                            AFU_Archivo.SaveAs(Archivo);
                        }
                        Limpiar_Catalogo_Generales();
                        Configuracion_Formulario(false);
                        //Session.Remove("Dt_Resguardantes");
                        //Grid_Resguardantes.Visible = false;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Alta de Bienes Muebles", "alert('Alta de Bien Mueble Exitosa');", true);
                        Llenar_DataSet_Resguardos_Bienes(Bien_Mueble);
                    }
                }
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN: Cancela la operación que esta en proceso (Alta o Actualizar) o Sale
    ///             del Formulario.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 29/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        if (Btn_Salir.AlternateText.Equals("Salir"))
        {
            Session["Dt_Resguardantes_BM"] = null;
            Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
        }
        else
        {  // Si el botón es Cancelar
            Grid_Resguardantes.Enabled = true;
            Configuracion_Formulario(false);
            Limpiar_Detalles();
            Session.Remove("Dt_Resguardantes_BM");
            Btn_Salir.AlternateText = "Salir";
            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
            Btn_Agregar_Donador.Visible = false;
            Btn_Buscar_RFC_Donador.Visible = false;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Agregar_Donador_Click
    ///DESCRIPCIÓN: Lanza el Modal para agregar un Nuevo Donador.
    ///PROPIEDADES:     
    ///CREO: Salvador Hernández Ramírez.
    ///FECHA_CREO: 01/Febrero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Agregar_Donador_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Limpiar_Catalogo_Donadores();
            Div_MPE_Donadores_Mensaje_Error.Visible = false;
            MPE_Donadores.Show();
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_RFC_Donador_Click
    ///DESCRIPCIÓN: Realiza un busqueda de Donador por RFC.
    ///PROPIEDADES:     
    ///CREO: Salvador Hernández Ramírez.
    ///FECHA_CREO: 01/Febrero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Buscar_RFC_Donador_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Hdf_Donador_ID.Value = "";
            if (Txt_RFC_Donador.Text.Trim().Length > 0)
            {
                String RFC_Donador = Txt_RFC_Donador.Text.Trim();
                Cls_Cat_Pat_Com_Donadores_Negocio Donador = new Cls_Cat_Pat_Com_Donadores_Negocio();
                Donador.P_RFC = Txt_RFC_Donador.Text.Trim();
                Donador = Donador.Consultar_Datos_Donador();
                if (Donador.P_Donador_ID != null && Donador.P_Donador_ID.Trim().Length > 0)
                {
                    Hdf_Donador_ID.Value = Donador.P_Donador_ID;
                    Txt_Nombre_Donador.Text = Donador.P_Apellido_Paterno + " " + Donador.P_Apellido_Materno + " " + Donador.P_Nombre;
                }
                else
                {
                    Lbl_Ecabezado_Mensaje.Text = "El Donador no ha sido encontrado.";
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }
            else
            {
                Lbl_Ecabezado_Mensaje.Text = "Es necesario introducir el RFC del Donador";
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

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_MPE_Donador_Aceptar_Click
    ///DESCRIPCIÓN: Da de alta el donador.
    ///PROPIEDADES:     
    ///CREO: Salvador Hernández Ramírez.
    ///FECHA_CREO: 01/Febrero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_MPE_Donador_Aceptar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Validar_Catalogo_Donador())
            {
                Cls_Cat_Pat_Com_Donadores_Negocio Donador = new Cls_Cat_Pat_Com_Donadores_Negocio();
                Donador.P_RFC = Txt_RFC_Donador_MPE.Text.Trim();
                Donador = Donador.Consultar_Datos_Donador();
                if (Donador.P_Donador_ID != null && Donador.P_Donador_ID.Trim().Length > 0)
                {
                    Lbl_MPE_Donadores_Cabecera_Error.Text = "Ya existe un donador con ese RFC.";
                    Lbl_MPE_Donadores_Mensaje_Error.Text = "";
                    Div_MPE_Donadores_Mensaje_Error.Visible = true;
                    MPE_Donadores.Show();
                }
                else
                {
                    Donador.P_Nombre = Txt_Nombre_Donador_MPE.Text.Trim();
                    Donador.P_Apellido_Paterno = Txt_Apellido_Paterno_Donador.Text.Trim();
                    Donador.P_Apellido_Materno = Txt_Apellido_Materno_Donador.Text.Trim();
                    Donador.P_Direccion = Txt_Direccion_Donador.Text.Trim();
                    Donador.P_Cuidad = Txt_Ciudad_Donador.Text.Trim();
                    Donador.P_Estado = Txt_Estado_Donador.Text.Trim();
                    Donador.P_Telefono = Txt_Telefono_Donador.Text.Trim();
                    Donador.P_Celular = Txt_Celular_Donador.Text.Trim();
                    Donador.P_CURP = Txt_CURP_Donador.Text.Trim();
                    Donador.P_RFC = Txt_RFC_Donador_MPE.Text.Trim();
                    Donador.P_Usuario = Cls_Sessiones.Nombre_Empleado;
                    Donador = Donador.Alta_Donador();
                    Donador = Donador.Consultar_Datos_Donador();
                    Hdf_Donador_ID.Value = Donador.P_Donador_ID;
                    Txt_RFC_Donador.Text = Donador.P_RFC;
                    Txt_Nombre_Donador.Text = Txt_Nombre_Donador.Text = Donador.P_Apellido_Paterno + " " + Donador.P_Apellido_Materno + " " + Donador.P_Nombre;
                    Limpiar_Catalogo_Donadores();
                    Div_MPE_Donadores_Mensaje_Error.Visible = false;
                    MPE_Donadores.Hide();
                    //Upd_Panel.Update();
                }
            }
        }
        catch (Exception Ex)
        {
            Lbl_MPE_Donadores_Cabecera_Error.Text = Ex.Message;
            Lbl_MPE_Donadores_Mensaje_Error.Text = "";
            Div_MPE_Donadores_Mensaje_Error.Visible = true;
            MPE_Donadores.Show();
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_MPE_Donador_Cancelar_Click
    ///DESCRIPCIÓN:          Evento utilizado para ocultar el modal 
    ///PROPIEDADES:     
    ///              
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           02/Febrero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_MPE_Donador_Cancelar_Click(object sender, ImageClickEventArgs e)
    {
        MPE_Donadores.Hide();
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cmb_Dependencias_SelectedIndexChanged
    ///DESCRIPCIÓN:          Evento utilizado para obtener el identificador de la dependencia que se selecciono y acceder al metodo para llenar el combo de los empleados
    ///PROPIEDADES:     
    ///                      1.  Dependencia_ID. el identificador de la dependencia en base a la que se va hacer la consulta
    ///              
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO: 02/Febrero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Cmb_Dependencias_SelectedIndexChanged(object sender, EventArgs e)
    {
        String Depencencia_Id = Cmb_Dependencias.SelectedItem.Value.Trim();
        Consultar_Empleados(Depencencia_Id);
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: IBtn_Imagen_Error_Click
    ///DESCRIPCIÓN: Evento utilizado para mostrar u ocultar el mensaje de error.
    ///PROPIEDADES:     
    ///CREO: Salvador Hernández Ramírez.
    ///FECHA_CREO: 04/Febrero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void IBtn_Imagen_Error_Click(object sender, ImageClickEventArgs e)
    {
        if (Lbl_Mensaje_Error.Visible == true)
        {
            Lbl_Mensaje_Error.Visible = false;
        }
        else if (Lbl_Mensaje_Error.Visible == false)
        {
            Lbl_Mensaje_Error.Visible = true;
        }
    }

    
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Limpiar_FileUpload_Click
    ///DESCRIPCIÓN: Limpia el FileUpload que carga los archivos
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 10/Junio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Limpiar_FileUpload_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Remover_Sesiones_Control_AsyncFileUpload(AFU_Archivo.ClientID);
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }


    #region Bienes Muebles Detalles

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Agregar_Resguardante_Click
    ///DESCRIPCIÓN: Agrega una nuevo Empleado Resguardante para este Bien Mueble.
    ///             (No aun en la Base de Datos)
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 29/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO: Salvador Hernàndez Ramírez
    ///CAUSA_MODIFICACIÓN: El DataGrid debe mostrar: Numero de Empleado, Clave del Producto
    ///Nombre de la Dependencia, Nombre del Area Y Comentarios
    ///*******************************************************************************
    protected void Btn_Agregar_Resguardante_Click(object sender, ImageClickEventArgs e)
    {
        if (Validar_Componentes_Resguardos())
        {
            DataTable Tabla = (DataTable)Grid_Resguardantes.DataSource;
            if (Tabla == null)
            {
                if (Session["Dt_Resguardantes_BM"] == null)
                {
                    Tabla = new DataTable("Resguardos");
                    Tabla.Columns.Add("EMPLEADO_ALMACEN_ID", Type.GetType("System.String"));
                    Tabla.Columns.Add("EMPLEADO_ID", Type.GetType("System.String"));
                    Tabla.Columns.Add("NO_EMPLEADO", Type.GetType("System.String"));
                    Tabla.Columns.Add("NOMBRE_EMPLEADO", Type.GetType("System.String"));
                    Tabla.Columns.Add("COMENTARIOS", Type.GetType("System.String"));
                }
                else
                {
                    Tabla = (DataTable)Session["Dt_Resguardantes_BM"];
                }
            }
            if (!Buscar_Clave_DataTable(Cmb_Empleados.SelectedItem.Value, Tabla, 1))
            {
                Cls_Cat_Empleados_Negocios Empleados_Negocio = new Cls_Cat_Empleados_Negocios();
                Empleados_Negocio.P_Empleado_ID = Cmb_Empleados.SelectedItem.Value;
                DataTable Dt_Empleado = Empleados_Negocio.Consulta_Datos_Empleado();
                if (Dt_Empleado != null && Dt_Empleado.Rows.Count > 0) {
                    DataRow Fila = Tabla.NewRow();
                    Fila["EMPLEADO_ALMACEN_ID"] = HttpUtility.HtmlDecode(Cls_Sessiones.Empleado_ID); // Se debe realizar una consulta para obtenerlo
                    Fila["EMPLEADO_ID"] = Dt_Empleado.Rows[0][Cat_Empleados.Campo_Empleado_ID].ToString().Trim();
                    Fila["NO_EMPLEADO"] = Dt_Empleado.Rows[0][Cat_Empleados.Campo_No_Empleado].ToString().Trim();
                    Fila["NOMBRE_EMPLEADO"] = HttpUtility.HtmlDecode(Cmb_Empleados.SelectedItem.Text);
                    if (Txt_Cometarios.Text.Trim().Length > 255) { Fila["COMENTARIOS"] = HttpUtility.HtmlDecode(Txt_Cometarios.Text.Trim().Substring(0, 254)); }
                    else { Fila["COMENTARIOS"] = HttpUtility.HtmlDecode(Txt_Cometarios.Text); }
                    Tabla.Rows.Add(Fila);
                }
                Grid_Resguardantes.Columns[1].Visible = true;
                Grid_Resguardantes.Columns[2].Visible = true;
                Grid_Resguardantes.DataSource = Tabla;
                Session["Dt_Resguardantes_BM"] = Tabla;
                Grid_Resguardantes.DataBind();
                Grid_Resguardantes.Columns[1].Visible = false;
                Grid_Resguardantes.Columns[2].Visible = false;
                Grid_Resguardantes.Visible = true;
                Grid_Resguardantes.SelectedIndex = (-1);
                Cmb_Empleados.SelectedIndex = 0;
                Txt_Cometarios.Text = "";
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
    ///FECHA_CREO: 29/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Quitar_Resguardante_Click(object sender, ImageClickEventArgs e)
    {
        if (Grid_Resguardantes.Rows.Count > 0 && Grid_Resguardantes.SelectedIndex > (-1))
        {
            Int32 Registro = ((Grid_Resguardantes.PageIndex) * Grid_Resguardantes.PageSize) + (Grid_Resguardantes.SelectedIndex);
            if (Session["Dt_Resguardantes_BM"] != null)
            {
                DataTable Tabla = (DataTable)Session["Dt_Resguardantes_BM"];
                Tabla.Rows.RemoveAt(Registro);
                Session["Dt_Resguardantes_BM"] = Tabla;
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

    #endregion

    #region Busqueda

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Limpiar_Filtros_Buscar_Datos_Click
    ///DESCRIPCIÓN: Maneja el Evento del Boton para realizar la Limpieza de los filtros
    ///             para la busqueda por parte de los Datos Generales.
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 01/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************    
    protected void Btn_Limpiar_Filtros_Buscar_Datos_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Txt_Busqueda_Inventario_Anterior.Text = "";
            Txt_Busqueda_Inventario_SIAS.Text = "";
            Txt_Busqueda_Producto.Text = "";
            Cmb_Busqueda_Dependencias.SelectedIndex = 0;
            Txt_Busqueda_Modelo.Text = "";
            Cmb_Busqueda_Marca.SelectedIndex = 0;
            Cmb_Busqueda_Estatus.SelectedIndex = 0;
            Txt_Busqueda_Numero_Serie.Text = "";
            Txt_Busqueda_Factura.Text = "";
            MPE_Busqueda_Bien_Mueble.Show();
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Datos_Click
    ///DESCRIPCIÓN: Maneja el Evento del Boton para realizar la Busqueda de los
    ///             Datos Generales.
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 01/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************    
    protected void Btn_Buscar_Datos_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Session["FILTRO_BUSQUEDA"] = "DATOS_GENERALES";
            Llenar_Grid_Listado_Bienes(0);
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Limpiar_Filtros_Buscar_Resguardante_Click
    ///DESCRIPCIÓN: Maneja el Evento del Boton para realizar la Limpieza de los filtros
    ///             para la busqueda por parte de los Listados.
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 01/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Btn_Limpiar_Filtros_Buscar_Resguardante_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Txt_Busqueda_RFC_Resguardante.Text = "";
            Cmb_Busqueda_Nombre_Resguardante.SelectedIndex = 0;
            Cmb_Busqueda_Resguardantes_Dependencias.SelectedIndex = 0;
            DataTable Tabla = new DataTable();
            Tabla.Columns.Add("EMPLEADO_ID", Type.GetType("System.String"));
            Tabla.Columns.Add("NOMBRE", Type.GetType("System.String"));
            Llenar_Combo_Empleados_Busqueda(Tabla);
            MPE_Busqueda_Bien_Mueble.Show();
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Resguardante_Click
    ///DESCRIPCIÓN: Maneja el Evento del Boton para realizar la Busqueda de los
    ///             Reguardante
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 01/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************    
    protected void Btn_Buscar_Resguardante_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Session["FILTRO_BUSQUEDA"] = "RESGUARDANTES";
            Llenar_Grid_Listado_Bienes(0);
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    #endregion

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cmb_Asignacion_Secundaria_SelectedIndexChanged
    ///DESCRIPCIÓN: Manipula la configuracion de los componentes dependiendo si van a 
    ///             depender de otro bien mueble.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 18/Marzo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Cmb_Asignacion_Secundaria_SelectedIndexChanged(object sender, EventArgs e) {
        try {
            Limpiar_Parent();
            Limpiar_Campos_Vehiculo_Parent();
            Session.Remove("Dt_Resguardantes");
            if (Cmb_Asignacion_Secundaria.SelectedIndex != 0) {
                if (Cmb_Asignacion_Secundaria.SelectedItem.Value.Equals("NINGUNA")) {
                    Div_Producto_Bien_Mueble_Padre.Visible = false;
                    Div_Vehiculo_Parent.Visible = false;
                    Div_Resguardos.Visible = true;
                    Cmb_Dependencias.Enabled = true;
                    Session.Remove("Dt_Resguardantes");
                    Grid_Resguardantes.DataSource = new DataTable();
                    Grid_Resguardantes.DataBind();
                } else if (Cmb_Asignacion_Secundaria.SelectedItem.Value.Equals("BIEN_MUEBLE")) {
                    Div_Producto_Bien_Mueble_Padre.Visible = true;
                    Div_Vehiculo_Parent.Visible = false;
                    Div_Resguardos.Visible = false;
                    Cmb_Dependencias.Enabled = false;
                    Session.Remove("Dt_Resguardantes");
                    Grid_Resguardantes.DataSource = new DataTable();
                    Grid_Resguardantes.DataBind();
                } else if (Cmb_Asignacion_Secundaria.SelectedItem.Value.Equals("VEHICULO")) {
                    Div_Vehiculo_Parent.Visible = true;
                    Div_Producto_Bien_Mueble_Padre.Visible = false;
                    Div_Resguardos.Visible = false;
                    Cmb_Dependencias.Enabled = false;
                    Session.Remove("Dt_Resguardantes");
                    Grid_Resguardantes.DataSource = new DataTable();
                    Grid_Resguardantes.DataBind();
                }
            } else {
                Div_Vehiculo_Parent.Visible = false;
                Div_Producto_Bien_Mueble_Padre.Visible = false;
                Div_Resguardos.Visible = false;
                Cmb_Dependencias.Enabled = false;
                Session.Remove("Dt_Resguardantes");
                Grid_Resguardantes.DataSource = new DataTable();
                Grid_Resguardantes.DataBind();
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
    ///NOMBRE DE LA FUNCIÓN: Btn_Lanzar_Mpe_Productos_Click
    ///DESCRIPCIÓN: Lanza buscador de producto.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 18/Marzo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Lanzar_Mpe_Productos_Click(object sender, ImageClickEventArgs e) {
        try  {
            Div_Contenedor_Msj_Error.Visible = false;
            Lbl_Mensaje_Error.Text = "";
            //Pnl_Busqueda.Visible = true;
            Mpe_Productos_Cabecera.Show();
        } catch (Exception Ex) {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Lanzar_Mpe_Proveedores_Click
    ///DESCRIPCIÓN: Lanza buscador de producto.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 18/Marzo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Lanzar_Mpe_Proveedores_Click(object sender, ImageClickEventArgs e)
    {
        try  {
            Div_Contenedor_Msj_Error.Visible = false;
            Lbl_Mensaje_Error.Text = "";
            //Pnl_Busqueda.Visible = true;
            Mpe_Proveedores_Cabecera.Show();
        } catch (Exception Ex) {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Lanzar_Buscar_Bien_Click
    ///DESCRIPCIÓN: Lanza buscador de Bien Mueble Padre.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 18/Marzo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Lanzar_Buscar_Bien_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Div_Contenedor_Msj_Error.Visible = false;
            Lbl_Mensaje_Error.Text = "";
            Pnl_Busqueda_Bien_Mueble.Visible = true;
            MPE_Busqueda_Bien_Mueble.Show();
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cmb_Busqueda_Resguardantes_Dependencias_SelectedIndexChanged
    ///DESCRIPCIÓN: Maneja el evento de cambio de Selección del Combo de Dependencias
    ///             del Modal de Busqueda (Parte de Resguardantes).
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 18/Marzo/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Cmb_Busqueda_Resguardantes_Dependencias_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Cmb_Busqueda_Resguardantes_Dependencias.SelectedIndex > 0)
            {
                Cls_Ope_Pat_Com_Bienes_Muebles_Negocio Combo = new Cls_Ope_Pat_Com_Bienes_Muebles_Negocio();
                Combo.P_Tipo_DataTable = "EMPLEADOS";
                Combo.P_Dependencia_ID = Cmb_Busqueda_Resguardantes_Dependencias.SelectedItem.Value.Trim();
                DataTable Tabla = Combo.Consultar_DataTable();
                Llenar_Combo_Empleados_Busqueda(Tabla);
            }
            else
            {
                DataTable Tabla = new DataTable();
                Tabla.Columns.Add("EMPLEADO_ID", Type.GetType("System.String"));
                Tabla.Columns.Add("NOMBRE", Type.GetType("System.String"));
                Llenar_Combo_Empleados_Busqueda(Tabla);
            }
            MPE_Busqueda_Bien_Mueble.Show();
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Generar_Reporte_Click
    ///DESCRIPCIÓN: Genera el reporte detallado.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 04/Junio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Generar_Reporte_Click(object sender, ImageClickEventArgs e) {
        try {
            if (Hdf_Bien_Mueble_ID.Value.Trim().Length > 0) {
                Cls_Ope_Pat_Com_Bienes_Muebles_Negocio Mueble = new Cls_Ope_Pat_Com_Bienes_Muebles_Negocio();
                Mueble.P_Bien_Mueble_ID = Hdf_Bien_Mueble_ID.Value;

                if (Session["OPERACION_BM"] != null)
                    Mueble.P_Operacion = Session["OPERACION_BM"].ToString().Trim();

                Mueble = Mueble.Consultar_Detalles_Bien_Mueble();
                Llenar_DataSet_Resguardos_Bienes(Mueble);
            } else {
                Lbl_Ecabezado_Mensaje.Text = "Es necesario.";
                Lbl_Mensaje_Error.Text = "Tener un Bien para Reimprimir el Resguardo";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        } catch (Exception Ex) {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    #region Mpe_Buscar_Producto

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Txt_Nombre_Producto_Buscar_TextChanged
        ///DESCRIPCIÓN: Maneja el evento de cambio de Texto del Nombre de Producto.
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 04/Julio/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Txt_Nombre_Producto_Buscar_TextChanged(object sender, EventArgs e) {
            try {
                Llenar_Grid_Productos(0);
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Txt_Nombre_Proveedor_Buscar_TextChanged
        ///DESCRIPCIÓN: Maneja el evento de cambio de Texto del Nombre del proveedor.
        ///PARAMETROS:     
        ///CREO:        Salvador Hernández Ramírez
        ///FECHA_CREO:  08/Agosto/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Txt_Nombre_Proveedor_Buscar_TextChanged(object sender, EventArgs e)
        {
            try
            {
                Llenar_Grid_Proveedores(0);
            }
            catch (Exception Ex)
            {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Ejecutar_Busqueda_Productos_Click
        ///DESCRIPCIÓN: Ejecuta la Busqueda de los productos.
        ///PARAMETROS:     
        ///CREO:        Salvador Hernández Ramírez
        ///FECHA_CREO:  12/Septiembre/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Ejecutar_Busqueda_Productos_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                Llenar_Grid_Productos(0);
            }
            catch (Exception Ex)
            {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Ejecutar_Busqueda_Proveedores_Click
        ///DESCRIPCIÓN: Ejecuta la Busqueda de los proveedores.
        ///PARAMETROS:     
        ///CREO:        Salvador Hernández Ramírez
        ///FECHA_CREO:  22/Septiembre/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Ejecutar_Busqueda_Proveedores_Click(object sender, ImageClickEventArgs e) {
            try {
                Llenar_Grid_Proveedores(0);
            } catch (Exception Ex)  {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

    #endregion
    
        #region "Busqueda Resguardantes"
    
            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Btn_Busqueda_Avanzada_Resguardante_Click
            ///DESCRIPCIÓN: Lanza la Busqueda Avanzada para el Resguardante.
            ///PARAMETROS:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 24/Octubre/2011 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************    
            protected void Btn_Busqueda_Avanzada_Resguardante_Click(object sender, ImageClickEventArgs e) {
                try {
                    MPE_Resguardante.Show();
                }catch (Exception Ex) {
                    Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }


            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Btn_Busqueda_Empleados_Click
            ///DESCRIPCIÓN: Ejecuta la Busqueda Avanzada para el Resguardante.
            ///PARAMETROS:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 24/Octubre/2011 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************    
            protected void Btn_Busqueda_Empleados_Click(object sender, EventArgs e) {
                try {
                    Grid_Busqueda_Empleados_Resguardo.PageIndex = 0;
                    Llenar_Grid_Busqueda_Empleados_Resguardo();
                    MPE_Resguardante.Show();
                }  catch (Exception Ex) {
                    Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }

        #endregion
    

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Btn_Buscar_Vehiculo_Click
        ///DESCRIPCIÓN          : Busca el Vehículo al cual se asignara el Bien Actual.
        ///PARAMETROS           : 
        ///CREO                 : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO           : 11/Noviembre/2011 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        protected void Btn_Buscar_Vehiculo_Click(object sender, ImageClickEventArgs e) {
            try {
                MPE_Busqueda_Vehiculo.Show();
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = "Excepción.";
                Lbl_Mensaje_Error.Text = "Ex:['" + Ex.Message + "']";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Datos_Vehiculo_Click
        ///DESCRIPCIÓN: Ejecuta la Busqueda de los Vehiculos por datos generales.
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 18/Noviembre/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Buscar_Datos_Vehiculo_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                Session["FILTRO_BUSQUEDA"] = "DATOS_GENERALES";
                Llenar_Grid_Listado_Bienes_Vehiculos(0);
            }
            catch (Exception Ex)
            {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }



    
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Btn_Limpiar_Filtros_Buscar_Datos_Vehiculo_Click
        ///DESCRIPCIÓN          : Hace la limpieza de los campos.
        ///PARAMETROS           : 
        ///CREO                 : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO           : 18/Noviembre/2011 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        protected void Btn_Limpiar_Filtros_Buscar_Datos_Vehiculo_Click(object sender, ImageClickEventArgs e) {
            try {
                Txt_Busqueda_Vehiculo_Numero_Inventario.Text = "";
                Txt_Busqueda_Vehiculo_Numero_Economico.Text = "";
                Txt_Busqueda_Vehiculo_Modelo.Text = "";
                Cmb_Busqueda_Vehiculo_Marca.SelectedIndex = 0;
                Cmb_Busqueda_Vehiculo_Tipo_Vehiculo.SelectedIndex = 0;
                Cmb_Busqueda_Vehiculo_Tipo_Combustible.SelectedIndex = 0;
                Txt_Busqueda_Vehiculo_Anio_Fabricacion.Text = "";
                Cmb_Busqueda_Vehiculo_Color.SelectedIndex = 0;
                Cmb_Busqueda_Vehiculo_Zonas.SelectedIndex = 0;
                Cmb_Busqueda_Vehiculo_Estatus.SelectedIndex = 0;
                Cmb_Busqueda_Vehiculo_Dependencias.SelectedIndex = 0;
                MPE_Busqueda_Vehiculo.Show();
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = "Excepción.";
                Lbl_Mensaje_Error.Text = "Ex:['" + Ex.Message + "']";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Btn_Limpiar_Filtros_Buscar_Resguardante_Vehiculo_Click
        ///DESCRIPCIÓN          : Hace la limpieza de los campos.
        ///PARAMETROS           : 
        ///CREO                 : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO           : 18/Noviembre/2011 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        protected void Btn_Limpiar_Filtros_Buscar_Resguardante_Vehiculo_Click(object sender, ImageClickEventArgs e)  {
            try {
                Txt_Busqueda_Vehiculo_RFC_Resguardante.Text = "";
                Txt_Busqueda_Vehiculo_No_Empleado.Text = "";
                Cmb_Busqueda_Vehiculo_Resguardantes_Dependencias.SelectedIndex = 0;
                Cmb_Busqueda_Vehiculo_Resguardantes_Dependencias_SelectedIndexChanged(Cmb_Busqueda_Vehiculo_Resguardantes_Dependencias, null);
                Cmb_Busqueda_Vehiculo_Nombre_Resguardante.SelectedIndex = 0;
                MPE_Busqueda_Vehiculo.Show();
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = "Excepción.";
                Lbl_Mensaje_Error.Text = "Ex:['" + Ex.Message + "']";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }
    
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Resguardante_Vehiculo_Click
        ///DESCRIPCIÓN: Ejecuta la Busqueda de los vehiculos por resguardos.
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 18/Noviembre/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Buscar_Resguardante_Vehiculo_Click(object sender, ImageClickEventArgs e) {
            try {
                Session["FILTRO_BUSQUEDA"] = "RESGUARDANTES";
                Llenar_Grid_Listado_Bienes_Vehiculos(0);
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Cmb_Busqueda_Vehiculo_Resguardantes_Dependencias_SelectedIndexChanged
        ///DESCRIPCIÓN          : Maneja el evento del Combo de Dependencias.
        ///PARAMETROS           : 
        ///CREO                 : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO           : 22/Noviembre/2011 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        protected void Cmb_Busqueda_Vehiculo_Resguardantes_Dependencias_SelectedIndexChanged(object sender, EventArgs e) {
            if (Cmb_Busqueda_Vehiculo_Resguardantes_Dependencias.SelectedIndex > 0)
            {
                Llenar_Combo_Empleados(Cmb_Busqueda_Vehiculo_Resguardantes_Dependencias.SelectedItem.Value.Trim(), ref Cmb_Busqueda_Vehiculo_Nombre_Resguardante);
            } else {
                Llenar_Combo_Empleados(null, ref Cmb_Busqueda_Vehiculo_Nombre_Resguardante);
            }
        }


    #endregion

}