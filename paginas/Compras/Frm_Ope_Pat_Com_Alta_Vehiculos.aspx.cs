using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using Presidencia.Control_Patrimonial_Operacion_Vehiculos.Negocio;
using Presidencia.Almacen_Resguardos.Negocio;
using Presidencia.Control_Patrimonial_Catalogo_Donadores.Negocio;
using Presidencia.Control_Patrimonial_Operacion_Partes_Vehiculos.Negocio;
using System.Collections.Generic;
using System.IO;
using Presidencia.Catalogo_Compras_Productos.Negocio;
using Presidencia.Reportes;
using System.Collections.Generic;
using Presidencia.Control_Patrimonial_Catalogo_Procedencias.Negocio;
using Presidencia.Catalogo_Compras_Proveedores.Negocio;
using Presidencia.Empleados.Negocios;
using Presidencia.Control_Patrimonial_Catalogo_Clasificaciones.Negocio;
using Presidencia.Control_Patrimonial_Catalogo_Clases_Activos.Negocio;

public partial class paginas_predial_Frm_Ope_Pat_Com_Alta_Vehiculos : System.Web.UI.Page
{

    #region Variables Internas

    String Fecha_Adquisicion = null;

    #endregion

    #region Page_Load

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Page_Load
    ///DESCRIPCIÓN: Metodo que se carga cada que ocurre un PostBack de la Página
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 03/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Page_Load(object sender, EventArgs e) {
        if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");
        Div_Contenedor_Msj_Error.Visible = false;
        if (!IsPostBack) {
            Div_Partes.Visible = false;
            Configuracion_Formulario(true, "VEHICULOS");
            Llenar_Combos();
            Llenar_Combo_Procedencias();
            Llenar_Grid_Productos(0);
            Llenar_Grid_Proveedores(0);
            Grid_Resguardos_Partes.Columns[1].Visible = false;
            Grid_Partes.Columns[1].Visible = false;
            String Producto_ID = null;
            String No_Requisicion = null;
            if (Request.QueryString["No_Requisicion"] != null) {
                No_Requisicion = HttpUtility.HtmlDecode(Request.QueryString["No_Requisicion"]).Trim();
                Session["No_Requisicion"] = No_Requisicion;
            }
            if (Request.QueryString["Producto_ID"] != null) { Producto_ID = HttpUtility.HtmlDecode(Request.QueryString["Producto_ID"]).Trim(); } //else { Producto_ID = "0000000007"; }
            if (Request.QueryString["Fecha_Adquisicion"] != null) { Fecha_Adquisicion = HttpUtility.HtmlDecode(Request.QueryString["Fecha_Adquisicion"]).Trim(); }
            if (Producto_ID != null) {
                Cargar_Datos_Producto(Producto_ID);
                Div_Generales_Producto.Visible = true;
                Div_Generales_Otra_Procedencia.Visible = false;
            } else {
                Div_Generales_Producto.Visible = false;
                Div_Generales_Otra_Procedencia.Visible = true;
            }
            Lbl_Capacidad_Carga.Visible = false;
            Txt_Capacidad_Carga.Visible = false;
        }
        //Configuracion_Acceso("Frm_Ope_Pat_Com_Alta_Vehiculos.aspx");
    }

    #endregion

    #region Metodos
    

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
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cargar_Datos_Producto
    ///DESCRIPCIÓN: Carga los Datos mas basicos del Vehículo pasandole el Producto.
    ///PROPIEDADES:  
    ///             1.  ID_Producto.  Identificador del Vehículo que se va a cargar.
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 28/Enero/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    public void Cargar_Datos_Producto(String ID_Producto)
    {
        try
        {
            Limpiar_Catalogo_Generales();
            Cls_Ope_Pat_Com_Vehiculos_Negocio Producto = new Cls_Ope_Pat_Com_Vehiculos_Negocio();
            Producto.P_Producto_ID = ID_Producto;
            Producto.P_Tipo_DataTable = "PRODUCTO";
            DataTable Dt_Detalles_Producto = Producto.Consultar_DataTable();
            Hdf_Producto_ID.Value = ID_Producto;
            Txt_Producto_ID.Text = ID_Producto.Trim();
            if (Dt_Detalles_Producto != null && Dt_Detalles_Producto.Rows.Count > 0)
            {
                Txt_Nombre_Producto.Text = Dt_Detalles_Producto.Rows[0]["NOMBRE_PRODUCTO"].ToString();
                Txt_Marca_Producto.Text = Dt_Detalles_Producto.Rows[0]["MARCA_PRODUCTO"].ToString();
                Txt_Modelo_Producto.Text = Dt_Detalles_Producto.Rows[0]["MODELO_PRODUCTO"].ToString();
                Txt_Proveedor_Producto.Text = Dt_Detalles_Producto.Rows[0]["PROVEEDOR_PRODUCTO"].ToString();
                Txt_Clave_Producto.Text = Dt_Detalles_Producto.Rows[0]["CLAVE_PRODUCTO"].ToString();
                Txt_Costo_Producto.Text = Dt_Detalles_Producto.Rows[0]["COSTO_PRODUCTO"].ToString();
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
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Empleados
    ///DESCRIPCIÓN: Llena el combo de Empleados.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 03/Diciembre/2010 
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
            Cmb_Resguardante_Parte.DataSource = Tabla;
            Cmb_Resguardante_Parte.DataValueField = "EMPLEADO_ID";
            Cmb_Resguardante_Parte.DataTextField = "NOMBRE";
            Cmb_Resguardante_Parte.DataBind();
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
    ///FECHA_CREO: 03/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Combos()
    {
        try
        {
            Cls_Ope_Pat_Com_Vehiculos_Negocio Combos = new Cls_Ope_Pat_Com_Vehiculos_Negocio();
            //SE LLENA EL COMBO DE DEPENDENCIAS
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
            Dependencias.Rows.RemoveAt(0);
            Cmb_Busqueda_Dependencia.DataSource = Dependencias;
            Cmb_Busqueda_Dependencia.DataValueField = "DEPENDENCIA_ID";
            Cmb_Busqueda_Dependencia.DataTextField = "NOMBRE";
            Cmb_Busqueda_Dependencia.DataBind();
            Cmb_Busqueda_Dependencia.Items.Insert(0, new ListItem("< TODAS >", ""));


            //SE LLENA EL COMBO DE TIPOS VEHICULOS
            Combos.P_Tipo_DataTable = "TIPOS_VEHICULOS";
            DataTable Tipos_Vehiculos = Combos.Consultar_DataTable();
            DataRow Fila_Tipo_Vehiculo = Tipos_Vehiculos.NewRow();
            Fila_Tipo_Vehiculo["TIPO_VEHICULO_ID"] = "SELECCIONE";
            Fila_Tipo_Vehiculo["DESCRIPCION"] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
            Tipos_Vehiculos.Rows.InsertAt(Fila_Tipo_Vehiculo, 0);
            Cmb_Tipos_Vehiculos.DataSource = Tipos_Vehiculos;
            Cmb_Tipos_Vehiculos.DataValueField = "TIPO_VEHICULO_ID";
            Cmb_Tipos_Vehiculos.DataTextField = "DESCRIPCION";
            Cmb_Tipos_Vehiculos.DataBind();

            //SE LLENA EL COMBO DE TIPOS COMBUSTIBLE
            Combos.P_Tipo_DataTable = "TIPOS_COMBUSTIBLE";
            DataTable Tipos_Combustible = Combos.Consultar_DataTable();
            DataRow Fila_Tipo_Combustible = Tipos_Combustible.NewRow();
            Fila_Tipo_Combustible["TIPO_COMBUSTIBLE_ID"] = "SELECCIONE";
            Fila_Tipo_Combustible["DESCRIPCION"] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
            Tipos_Combustible.Rows.InsertAt(Fila_Tipo_Combustible, 0);
            Cmb_Tipo_Combustible.DataSource = Tipos_Combustible;
            Cmb_Tipo_Combustible.DataValueField = "TIPO_COMBUSTIBLE_ID";
            Cmb_Tipo_Combustible.DataTextField = "DESCRIPCION";
            Cmb_Tipo_Combustible.DataBind();

            //SE LLENA EL COMBO DE ZONAS
            Combos.P_Tipo_DataTable = "ZONAS";
            DataTable Zonas = Combos.Consultar_DataTable();
            DataRow Fila_Zona = Zonas.NewRow();
            Fila_Zona["ZONA_ID"] = "SELECCIONE";
            Fila_Zona["DESCRIPCION"] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
            Zonas.Rows.InsertAt(Fila_Zona, 0);
            Cmb_Zonas.DataSource = Zonas;
            Cmb_Zonas.DataTextField = "DESCRIPCION";
            Cmb_Zonas.DataValueField = "ZONA_ID";
            Cmb_Zonas.DataBind();

            //SE LLENA EL COMBO DE MARCAS
            Combos.P_Tipo_DataTable = "MARCAS";
            DataTable Marcas = Combos.Consultar_DataTable();
            DataRow Fila_Marca = Marcas.NewRow();
            Fila_Marca["MARCA_ID"] = "SELECCIONE";
            Fila_Marca["NOMBRE"] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
            Marcas.Rows.InsertAt(Fila_Marca, 0);
            Cmb_Marca_Parte.DataSource = Marcas;
            Cmb_Marca_Parte.DataTextField = "NOMBRE";
            Cmb_Marca_Parte.DataValueField = "MARCA_ID";
            Cmb_Marca_Parte.DataBind();
            Cmb_Marca.DataSource = Marcas;
            Cmb_Marca.DataTextField = "NOMBRE";
            Cmb_Marca.DataValueField = "MARCA_ID";
            Cmb_Marca.DataBind();

            //SE LLENA EL COMBO DE MATERIALES
            Combos.P_Tipo_DataTable = "MATERIALES";
            DataTable Materiales = Combos.Consultar_DataTable();
            DataRow Fila_Material = Materiales.NewRow();
            Fila_Material["MATERIAL_ID"] = "SELECCIONE";
            Fila_Material["DESCRIPCION"] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
            Materiales.Rows.InsertAt(Fila_Material, 0);
            Cmb_Material_Parte.DataSource = Materiales;
            Cmb_Material_Parte.DataTextField = "DESCRIPCION";
            Cmb_Material_Parte.DataValueField = "MATERIAL_ID";
            Cmb_Material_Parte.DataBind();

            //SE LLENA LOS COMBOS DE COLORES
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
            Cmb_Color_Parte.DataSource = Colores;
            Cmb_Color_Parte.DataValueField = "COLOR_ID";
            Cmb_Color_Parte.DataTextField = "DESCRIPCION";
            Cmb_Color_Parte.DataBind();

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
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Procedencias
    ///DESCRIPCIÓN: Llena el combo de Procedencias.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 07/Julio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Combo_Procedencias()
    {
        Cls_Cat_Pat_Com_Procedencias_Negocio Procedencia_Negocio = new Cls_Cat_Pat_Com_Procedencias_Negocio();
        Procedencia_Negocio.P_Estatus = "VIGENTE";
        Procedencia_Negocio.P_Tipo_DataTable = "PROCEDENCIAS";
        Cmb_Procedencia.DataSource = Procedencia_Negocio.Consultar_DataTable();
        Cmb_Procedencia.DataTextField = "NOMBRE";
        Cmb_Procedencia.DataValueField = "PROCEDENCIA_ID";
        Cmb_Procedencia.DataBind();
        Cmb_Procedencia.Items.Insert(0, new ListItem("<SELECCIONE>", ""));
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Configuracion_Formulario
    ///DESCRIPCIÓN: Carga una configuracion de los controles del Formulario
    ///PROPIEDADES:     
    ///             1. Estatus. Estatus en el que se cargara la configuración de los 
    ///                         controles.
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 03/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Configuracion_Formulario(Boolean Estatus, String Tipo_Formulario)
    {
        if (Tipo_Formulario.Trim().Equals("VEHICULOS"))
        {
            Div_Partes.Visible = false;
            Div_Resguardos.Visible = true;
            Div_Datos_Generales.Visible = true;
            if (Estatus)
            {
                Btn_Nuevo.AlternateText = "Nuevo";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                Btn_Salir.AlternateText = "Salir";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                Btn_Generar_Reporte.Visible = true;
            }
            else
            {
                Btn_Nuevo.AlternateText = "Dar de Alta";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                Btn_Salir.AlternateText = "Cancelar";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                Btn_Generar_Reporte.Visible = false;
            }
            Cmb_Tipo_Activo.Enabled = !Estatus;
            Cmb_Clase_Activo.Enabled = !Estatus;
            Btn_Agregar_Donador.Visible = !Estatus;
            Btn_Lanzar_Mpe_Productos.Visible = !Estatus;
            Btn_Buscar_RFC_Donador.Visible = !Estatus;
            Txt_Modelo.Enabled = !Estatus;
            Cmb_Marca.Enabled = !Estatus;
            Cmb_Dependencias.Enabled = !Estatus;
            Cmb_Tipos_Vehiculos.Enabled = !Estatus;
            Cmb_Tipo_Combustible.Enabled = !Estatus;
            Cmb_Colores.Enabled = !Estatus;
            Cmb_Zonas.Enabled = !Estatus;
            Txt_Numero_Inventario.Enabled = false;
            Txt_Numero_Economico.Enabled = !Estatus;
            Txt_Placas.Enabled = !Estatus;
            Txt_Costo.Enabled = !Estatus;
            Txt_Capacidad_Carga.Enabled = !Estatus;
            Txt_Anio_Fabricacion.Enabled = !Estatus;
            Txt_Serie_Carroceria.Enabled = !Estatus;
            Txt_Numero_Cilindros.Enabled = !Estatus;
            Btn_Fecha_Adquisicion.Enabled = !Estatus;
            Txt_Kilometraje.Enabled = !Estatus;
            Txt_No_Factura.Enabled = !Estatus;
            Btn_Lanzar_Mpe_Proveedores.Visible = !Estatus;
            Cmb_Odometro.Enabled = !Estatus;
            Txt_Observaciones.Enabled = !Estatus;
            Cmb_Empleados.Enabled = !Estatus;
            Txt_Cometarios.Enabled = !Estatus;
            Btn_Agregar_Resguardante.Visible = !Estatus;
            Btn_Quitar_Resguardante.Visible = !Estatus;
            Cmb_Procedencia.Enabled = !Estatus;
            AFU_Archivo.Enabled = !Estatus;
            Grid_Resguardantes.Columns[0].Visible = !Estatus;
            Btn_Busqueda_Avanzada_Resguardante.Visible = !Estatus;
            Btn_Limpiar_Donador.Visible = !Estatus;
        }
        else if (Tipo_Formulario.Trim().Equals("PARTES"))
        {
            Div_Resguardos.Visible = false;
            Div_Datos_Generales.Visible = false;
            Div_Partes.Visible = true;
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
            Cmb_Dependencias.Enabled = false;
            Cmb_Tipos_Vehiculos.Enabled = false;
            Cmb_Tipo_Combustible.Enabled = false;
            Cmb_Colores.Enabled = false;
            Cmb_Zonas.Enabled = false;
            Txt_Numero_Inventario.Enabled = false;
            Txt_Numero_Economico.Enabled = false;
            Txt_Placas.Enabled = false;
            Txt_Costo.Enabled = false;
            Txt_Capacidad_Carga.Enabled = false;
            Txt_Anio_Fabricacion.Enabled = false;
            Txt_Serie_Carroceria.Enabled = false;
            Txt_Numero_Cilindros.Enabled = false;
            Btn_Fecha_Adquisicion.Enabled = false;
            Txt_Kilometraje.Enabled = false;
            Cmb_Odometro.Enabled = false;
            Txt_Observaciones.Enabled = false;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Limpiar_Catalogo
    ///DESCRIPCIÓN: Limpia los controles Generales del Formulario
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 03/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Limpiar_Catalogo_Generales()
    {
        Hdf_Producto_ID.Value = "";
        Hdf_Vehiculo_ID.Value = "";
        Cmb_Procedencia.SelectedIndex = 0;
        Txt_Producto_ID.Text = "";
        Txt_Nombre_Producto_Donado.Text = "";
        Txt_Nombre_Producto.Text = "";
        Txt_Marca_Producto.Text = "";
        Cmb_Marca.SelectedIndex = 0;
        Txt_Proveedor_Producto.Text = "";
        Txt_Clave_Producto.Text = "";
        Txt_Costo_Producto.Text = "";
        Cmb_Procedencia.SelectedIndex = 0;
        Txt_Nombre_Producto_Donado.Text = "";
        Cmb_Marca.SelectedIndex = 0;
        Txt_Modelo.Text = "";
        Txt_Modelo_Producto.Text = "";
        Hdf_Donador_ID.Value = "";
        Txt_RFC_Donador.Text = "";
        Txt_Nombre_Donador.Text = "";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Limpiar_Detalles
    ///DESCRIPCIÓN: Limpia los controles de detalles del Formulario
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 03/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Limpiar_Detalles()
    {
        Cmb_Dependencias.SelectedIndex = 0;
        Cmb_Clase_Activo.SelectedIndex = 0;
        Cmb_Tipo_Activo.SelectedIndex = 0;
        Cmb_Tipos_Vehiculos.SelectedIndex = 0;
        Cmb_Tipo_Combustible.SelectedIndex = 0;
        Cmb_Colores.SelectedIndex = 0;
        Cmb_Zonas.SelectedIndex = 0;
        Txt_Numero_Inventario.Text = "";
        Txt_Numero_Economico.Text = "";
        Txt_Placas.Text = "";
        Txt_Costo.Text = "";
        Txt_Capacidad_Carga.Text = "";
        Txt_Anio_Fabricacion.Text = "";
        Txt_Serie_Carroceria.Text = "";
        Txt_Numero_Cilindros.Text = "";
        Txt_Fecha_Adquisicion.Text = "";
        Txt_Kilometraje.Text = "";
        Txt_No_Factura.Text = "";
        Txt_Nombre_Proveedor.Text = "";
        Hdf_Proveedor_ID.Value = "";
        Cmb_Estatus.SelectedIndex = 0;
        Cmb_Odometro.SelectedIndex = 0;
        Txt_Observaciones.Text = "";
        Cmb_Empleados.SelectedIndex = 0;
        Txt_Cometarios.Text = "";
        Grid_Resguardantes.SelectedIndex = -1;
        Grid_Resguardantes.DataSource = new DataTable();
        Grid_Resguardantes.DataBind();
        Remover_Sesiones_Control_AsyncFileUpload(AFU_Archivo.ClientID);
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Limpiar_Catalogo_Donadores
    ///DESCRIPCIÓN: Limpia los controles del Formulario
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 22/Enero/2010 
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
    ///NOMBRE DE LA FUNCIÓN: Limpiar_Generales
    ///DESCRIPCIÓN: Limpia la parte general de las partes.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 26/Febrero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    public void Limpiar_Producto()
    {
        Hdf_Producto_Parte_ID.Value = "";
        Txt_Nombre_Parte.Text = "";
        Cmb_Marca_Parte.SelectedIndex = 0;
        Cmb_Modelo_Parte.SelectedIndex = 0;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Limpiar_Partes
    ///DESCRIPCIÓN: Prepara el Formulario para agregarle las partes a los vehiculos.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 01/Febrero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Limpiar_Partes()
    {
        Hdf_Parte_ID.Value = "";
        Txt_Nombre_Parte.Text = "";
        Txt_Numero_Inventario_Parte.Text = "";
        Cmb_Material_Parte.SelectedIndex = 0;
        Cmb_Color_Parte.SelectedIndex = 0;
        Cmb_Marca_Parte.SelectedIndex = 0;
        Cmb_Modelo_Parte.SelectedIndex = 0;
        Txt_Costo_Parte.Text = "";
        Txt_Fecha_Adquisicion_Parte.Text = "";
        Cmb_Estado_Parte.SelectedIndex = 0;
        Txt_Comentarios_Parte.Text = "";
        Cmb_Resguardante_Parte.SelectedIndex = 0;
        Txt_Comentarios_Resguardo_Parte.Text = "";
        Grid_Resguardos_Partes.SelectedIndex = -1;
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
    ///FECHA_CREO: 03/Diciembre/2010 
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
    ///NOMBRE DE LA FUNCIÓN: Llenar_DataSet_Resguardos_Vehiculos
    ///DESCRIPCIÓN: Llena el dataSet "Data_Set_Resguardos_Vehiculos" con las personas a las que se les asigno el
    ///vehiculo, sus detalles generales y especificos, para que con estos datos se genere el reporte.
    ///PARAMETROS:  
    ///CREO: Salvador Hernández Ramírez
    ///FECHA_CREO: 23/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Llenar_DataSet_Resguardos_Vehiculos(Cls_Ope_Pat_Com_Vehiculos_Negocio Id_Vehiculo)
    {
        try
        {

            String Formato = "PDF";
            Id_Vehiculo.P_Producto_Almacen = false;
            Cls_Alm_Com_Resguardos_Negocio Consulta_Resguardos_Vehiculos = new Cls_Alm_Com_Resguardos_Negocio();
            DataSet Data_Set_Resguardos_Vehiculos, Data_Set_Vehiculos_Asegurados;
            Data_Set_Resguardos_Vehiculos = Consulta_Resguardos_Vehiculos.Consulta_Resguardos_Vehiculos(Id_Vehiculo);
            Data_Set_Vehiculos_Asegurados = Consulta_Resguardos_Vehiculos.Consulta_Vehiculos_Asegurados(Id_Vehiculo);
            Ds_Alm_Com_Resguardos_Vehiculos Ds_Consulta_Resguardos_Vehiculos = new Ds_Alm_Com_Resguardos_Vehiculos();
            Generar_Reporte(Data_Set_Vehiculos_Asegurados, Data_Set_Resguardos_Vehiculos, Ds_Consulta_Resguardos_Vehiculos, Formato);
        } catch (Exception Ex) {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Generar_Reporte
    ///DESCRIPCIÓN: caraga el data set fisico con el cual se genera el Reporte especificado
    ///PARAMETROS:  1.-Data_Set_Consulta_DB.- Contiene la informacion de la consulta a la base de datos
    ///             2.-Ds_Reporte, Objeto que contiene la instancia del Data set fisico del Reporte a generar
    ///             3.-Nombre_Reporte, contiene el nombre del Reporte a mostrar en pantalla
    ///CREO: Salvador Hernández Ramírez
    ///FECHA_CREO: 15/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Generar_Reporte(DataSet Data_Set_Consulta_Vehiculos_A, DataSet Data_Set_Consulta_Resguardos_V, DataSet Ds_Reporte, String Formato)
    {
        String Ruta_Reporte_Crystal = "";
        String Nombre_Reporte_Generar = "";
        DataRow Renglon;

        try
        {
            if (Data_Set_Consulta_Resguardos_V.Tables[0].Rows.Count > 0)
            {
                String Cantidad = Data_Set_Consulta_Resguardos_V.Tables[0].Rows[0]["CANTIDAD"].ToString();
                String Costo = Data_Set_Consulta_Resguardos_V.Tables[0].Rows[0]["COSTO_UNITARIO"].ToString();
                Double Resultado = (Convert.ToDouble(Cantidad)) * (Convert.ToDouble(Costo));

                String Total = "" + Resultado;
                Renglon = Data_Set_Consulta_Resguardos_V.Tables[0].Rows[0];
                Ds_Reporte.Tables[1].ImportRow(Renglon);
                Ds_Reporte.Tables[1].Rows[0].SetField("COSTO_TOTAL", Total);

                for (int Cont_Elementos = 0; Cont_Elementos < Data_Set_Consulta_Resguardos_V.Tables[0].Rows.Count; Cont_Elementos++)
                {
                    Renglon = Data_Set_Consulta_Resguardos_V.Tables[0].Rows[Cont_Elementos]; //Instanciar renglon e importarlo
                    Ds_Reporte.Tables[0].ImportRow(Renglon);

                    String Nombre_E = Data_Set_Consulta_Resguardos_V.Tables[0].Rows[Cont_Elementos]["NOMBRE_E"].ToString();
                    String Apellido_Paterno_E = Data_Set_Consulta_Resguardos_V.Tables[0].Rows[Cont_Elementos]["APELLIDO_PATERNO_E"].ToString();
                    String Apellido_Materno_E = Data_Set_Consulta_Resguardos_V.Tables[0].Rows[Cont_Elementos]["APELLIDO_MATERNO_E"].ToString();
                    String RFC_E = Data_Set_Consulta_Resguardos_V.Tables[0].Rows[Cont_Elementos]["RFC_E"].ToString();
                    String Resguardante = Nombre_E + " " + Apellido_Paterno_E + " " + Apellido_Materno_E + " " + "(" + RFC_E + ")";
                    Ds_Reporte.Tables[0].Rows[Cont_Elementos].SetField("RESGUARDANTES", Resguardante);
                }

                if (Data_Set_Consulta_Vehiculos_A.Tables[0].Rows.Count > 0)
                {
                    String Nombre_Aeguradora = Data_Set_Consulta_Vehiculos_A.Tables[0].Rows[0]["NOMBRE_ASEGURADORA"].ToString();
                    String No_Poliza = Data_Set_Consulta_Vehiculos_A.Tables[0].Rows[0]["NO_POLIZA"].ToString();
                    String Descripcion_Seguro = Data_Set_Consulta_Vehiculos_A.Tables[0].Rows[0]["DESCRIPCION_SEGURO"].ToString();
                    String Cobertura = Data_Set_Consulta_Vehiculos_A.Tables[0].Rows[0]["COBERTURA"].ToString();
                    Ds_Reporte.Tables[1].Rows[0].SetField("NOMBRE_ASEGURADORA", Nombre_Aeguradora);
                    Ds_Reporte.Tables[1].Rows[0].SetField("NO_POLIZA", No_Poliza);
                    Ds_Reporte.Tables[1].Rows[0].SetField("DESCRIPCION_SEGURO", Descripcion_Seguro);
                    Ds_Reporte.Tables[1].Rows[0].SetField("COBERTURA", Cobertura);
                }
            }

            // Ruta donde se encuentra el Reporte Crystal
            Ruta_Reporte_Crystal = "../Rpt/Compras/Rpt_Alm_Com_Resguardos_Vehiculos.rpt";

            // Se crea el nombre del reporte
            String Nombre_Reporte = "Rpt_Resguardo_Vehiculos_" + Cls_Sessiones.No_Empleado + "_" + Convert.ToString(DateTime.Now.ToString("yyyy'-'MM'-'dd'_t'HH'-'mm'-'ss"));

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
    /// FECHA MODIFICO:      23-Mayo-2011
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


    #region Grids

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Resguardantes
    ///DESCRIPCIÓN: Llena la tabla de Resguardantes
    ///PROPIEDADES:     
    ///             1.  Pagina. Pagina en la cual se mostrará el Grid_VIew
    ///             2.  tabla.  Tabla que se va a cargar en el Grid.    
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 03/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Grid_Resguardantes(Int32 Pagina, DataTable Tabla)
    {
        Session["Dt_Resguardantes"] = Tabla;
        Grid_Resguardantes.Columns[1].Visible = true;
        Grid_Resguardantes.Columns[2].Visible = true;
        Grid_Resguardantes.DataSource = Tabla;
        Grid_Resguardantes.PageIndex = Pagina;
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
    ///FECHA_CREO: 03/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private Boolean Validar_Componentes_Generales()
    {
        Lbl_Ecabezado_Mensaje.Text = "Es necesario.";
        String Mensaje_Error = "";
        Boolean Validacion = true;
        if (Cmb_Procedencia.SelectedIndex == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Debe seleccionar una opción del Combo de Procedencia.";
            Validacion = false;
        }
        if (Hdf_Producto_ID.Value.Trim().Length == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Seleccionar el Producto.";
            Validacion = false;
        }
        if (Cmb_Marca.SelectedIndex == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Debe seleccionar una opción del Combo de Marca.";
            Validacion = false;
        }
        if (Txt_Modelo.Text.Trim().Length == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Debe seleccionar una opción del Combo de Modelo.";
            Validacion = false;
        }
        if (Validacion)
        {
            if (Cmb_Dependencias.SelectedIndex == 0)
            {
                if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                Mensaje_Error = Mensaje_Error + "+ Debe seleccionar una opción del Combo de Unidad Responsable.";
                Validacion = false;
            }
            if (Cmb_Tipos_Vehiculos.SelectedIndex == 0)
            {
                if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                Mensaje_Error = Mensaje_Error + "+ Debe seleccionar una opción del Combo de Tipos de Vehículos.";
                Validacion = false;
            }
            if (Cmb_Tipo_Combustible.SelectedIndex == 0)
            {
                if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                Mensaje_Error = Mensaje_Error + "+ Debe seleccionar una opción del Combo de Tipos de Combustible.";
                Validacion = false;
            }
            if (Cmb_Colores.SelectedIndex == 0)
            {
                if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                Mensaje_Error = Mensaje_Error + "+ Debe seleccionar una opción del Combo de Colores.";
                Validacion = false;
            }
            if (Cmb_Zonas.SelectedIndex == 0)
            {
                if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                Mensaje_Error = Mensaje_Error + "+ Debe seleccionar una opción del Combo de Zonas.";
                Validacion = false;
            }

            if (Txt_Numero_Economico.Text.Trim().Length == 0)
            {
                if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                Mensaje_Error = Mensaje_Error + "+ Introducir el Número Económico del Vehículo.";
                Validacion = false;
            }
            if (Txt_Placas.Text.Trim().Length == 0)
            {
                if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                Mensaje_Error = Mensaje_Error + "+ Introducir las Placas del Vehículo.";
                Validacion = false;
            }
            if (Txt_Costo.Text.Trim().Length == 0)
            {
                if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                Mensaje_Error = Mensaje_Error + "+ Introducir el Costo Actual del Vehículo.";
                Validacion = false;
            }
            if (Txt_Anio_Fabricacion.Text.Trim().Length == 0)
            {
                if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                Mensaje_Error = Mensaje_Error + "+ Introducir el Año de Fabricación del Vehículo.";
                Validacion = false;
            }
            if (Txt_Serie_Carroceria.Text.Trim().Length == 0)
            {
                if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                Mensaje_Error = Mensaje_Error + "+ Introducir el Número de Serie del Vehículo.";
                Validacion = false;
            }
            if (Txt_Numero_Cilindros.Text.Trim().Length == 0)
            {
                if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                Mensaje_Error = Mensaje_Error + "+ Introducir el Número de Cilindros del Vehículo.";
                Validacion = false;
            }
            if (Txt_Fecha_Adquisicion.Text.Trim().Length == 0)
            {
                if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                Mensaje_Error = Mensaje_Error + "+ Introducir la Fecha de Adquisición del Vehículo.";
                Validacion = false;
            }
            if (Txt_Kilometraje.Text.Trim().Length == 0)
            {
                if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                Mensaje_Error = Mensaje_Error + "+ Introducir el Kilometraje del Vehículo.";
                Validacion = false;
            }
            if (Cmb_Odometro.SelectedIndex == 0)
            {
                if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                Mensaje_Error = Mensaje_Error + "+ Debe seleccionar una opción del Combo de Odomentro.";
                Validacion = false;
            }
            if (Grid_Resguardantes.Rows.Count == 0 || Session["Dt_Resguardantes"] == null)
            {
                if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                Mensaje_Error = Mensaje_Error + "+ Debe haber como minimo un empleado para resguardo del Vehículo.";
                Validacion = false;
            }
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
    ///             una operación de la pestaña de Resguardos.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 03/Diciembre/2010 
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

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Catalogo_Donador
    ///DESCRIPCIÓN: Hace una validacion de que haya datos en los componentes antes de hacer
    ///             una operación para dar de alta un donador.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 22/Enero/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private Boolean Validar_Catalogo_Donador()
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
    ///NOMBRE DE LA FUNCIÓN: Validar_Partes
    ///DESCRIPCIÓN: Hace una validacion de que haya datos en los componentes antes de hacer
    ///             una operación para dar de alta una parte.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 01/Febrero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private Boolean Validar_Partes()
    {
        Lbl_MPE_Donadores_Cabecera_Error.Text = "Es necesario.";
        String Mensaje_Error = "";
        Boolean Validacion = true;
        if (Hdf_Producto_Parte_ID.Value.Trim().Length == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Seleccionar una Parte (Producto).";
            Validacion = false;
        }
        if (Hdf_Vehiculo_ID.Value.Trim().Length == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Seleccionar una Vehiculo (Al que se va a agregar la parte).";
            Validacion = false;
        }
        if (Txt_Numero_Inventario_Parte.Text.Trim().Length == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Introducir el Número de Inventario de la Parte.";
            Validacion = false;
        }

        if (Cmb_Material_Parte.SelectedIndex == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Seleccionar una opción del Combo de Material de la Parte.";
            Validacion = false;
        }
        if (Cmb_Color_Parte.SelectedIndex == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Seleccionar una opción del Combo de Color de la Parte.";
            Validacion = false;
        }
        if (Txt_Costo_Parte.Text.Trim().Length == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Introducir el Costo de la Parte.";
            Validacion = false;
        }
        if (Txt_Fecha_Adquisicion_Parte.Text.Trim().Length == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Introducir la Fecha de Adquisición de la Parte.";
            Validacion = false;
        }
        if (Cmb_Estado_Parte.SelectedIndex == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Debe seleccionar una opción del Combo de Estado de la Parte.";
            Validacion = false;
        }
        if (Cmb_Estatus_Parte.SelectedIndex == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Debe seleccionar una opción del Combo de Estatus de la Parte.";
            Validacion = false;
        }
        if (Txt_Comentarios_Parte.Text.Trim().Length > 500)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Los Comentarios de la Parte no pueden exceder los 500 carácteres (Sobrepasa por " + (Txt_Comentarios_Parte.Text.Trim().Length - 500) + ").";
            Validacion = false;
        }
        if (Grid_Resguardos_Partes.Rows.Count == 0 || Session["Dt_Resguardados"] == null)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Debe haber como minimo un empleado para resguardo de la Parte.";
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
    ///NOMBRE DE LA FUNCIÓN: Validar_Componentes_Resguardos_Partes
    ///DESCRIPCIÓN: Hace una validacion de que haya datos en los componentes antes de hacer
    ///             una operación de la pestaña de Resguardos de las Partes.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 02/Febrero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private Boolean Validar_Componentes_Resguardos_Partes()
    {
        Lbl_Ecabezado_Mensaje.Text = "Es necesario.";
        String Mensaje_Error = "";
        Boolean Validacion = true;
        if (Cmb_Resguardante_Parte.SelectedIndex == 0)
        {
            Mensaje_Error = Mensaje_Error + "+ Seleccionar el Empleado para Resguardo de la Parte.";
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

    #region Partes-Vehiculo

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Partes
    ///DESCRIPCIÓN: Llena la tabla de Resguardantes
    ///PROPIEDADES:     
    ///             1.  Lista_Partes. Lista de objetos de donde se llenará lista.
    ///             1.  Pagina. Pagina donde se establecera el Grid despues de llenarlo.
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 31/Enero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Grid_Partes(List<Cls_Ope_Pat_Com_Partes_Vehiculos_Negocio> Lista_Partes, Int32 Pagina)
    {
        try
        {
            Grid_Partes.Columns[1].Visible = true;
            DataTable Tabla = new DataTable();
            Tabla.Columns.Add("PARTE_ID", Type.GetType("System.Int32"));
            Tabla.Columns.Add("NOMBRE", Type.GetType("System.String"));
            Tabla.Columns.Add("MARCA", Type.GetType("System.String"));
            Tabla.Columns.Add("MODELO", Type.GetType("System.String"));
            Tabla.Columns.Add("CANTIDAD", Type.GetType("System.Int32"));
            foreach (Cls_Ope_Pat_Com_Partes_Vehiculos_Negocio Parte in Lista_Partes)
            {
                DataRow Fila = Tabla.NewRow();
                Fila["PARTE_ID"] = Parte.P_Parte_ID;
                Fila["NOMBRE"] = Parte.P_Nombre;
                Fila["MARCA"] = Parte.P_Marca;
                Fila["MODELO"] = Parte.P_Modelo;
                Fila["CANTIDAD"] = Parte.P_Cantidad;
                Tabla.Rows.Add(Fila);
            }
            Grid_Partes.SelectedIndex = (-1);
            Grid_Partes.DataSource = Tabla;
            Grid_Partes.PageIndex = Pagina;
            Grid_Partes.DataBind();
            Grid_Partes.Columns[1].Visible = false;
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Resguardos_Partes
    ///DESCRIPCIÓN: Llena la tabla de Resguardantes de las Partes
    ///PROPIEDADES:     
    ///             1.  Pagina. Pagina en la cual se mostrará el Grid_VIew
    ///             2.  tabla.  Tabla que se va a cargar en el Grid.    
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 02/Febrero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Grid_Resguardos_Partes(Int32 Pagina, DataTable Tabla)
    {
        Grid_Resguardos_Partes.Columns[1].Visible = true;
        Grid_Resguardos_Partes.DataSource = Tabla;
        Grid_Resguardos_Partes.PageIndex = Pagina;
        Grid_Resguardos_Partes.DataBind();
        Grid_Resguardos_Partes.Columns[1].Visible = false;
        Session["Dt_Resguardados"] = Tabla;
    }

    #endregion

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Mostrar_Detalles_Vehiculo
    ///DESCRIPCIÓN: Muestra a Detalle un Vehiculo.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 01/Febrero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Mostrar_Detalles_Vehiculo(Cls_Ope_Pat_Com_Vehiculos_Negocio Vehiculo)
    {
        try
        {
            Hdf_Vehiculo_ID.Value = Vehiculo.P_Vehiculo_ID;
            Cmb_Clase_Activo.SelectedIndex = Cmb_Clase_Activo.Items.IndexOf(Cmb_Clase_Activo.Items.FindByValue(Vehiculo.P_Clase_Activo_ID));
            Cmb_Tipo_Activo.SelectedIndex = Cmb_Tipo_Activo.Items.IndexOf(Cmb_Tipo_Activo.Items.FindByValue(Vehiculo.P_Clasificacion_ID));
            Cmb_Dependencias.SelectedIndex = Cmb_Dependencias.Items.IndexOf(Cmb_Dependencias.Items.FindByValue(Vehiculo.P_Dependencia_ID));
            Cmb_Tipos_Vehiculos.SelectedIndex = Cmb_Tipos_Vehiculos.Items.IndexOf(Cmb_Tipos_Vehiculos.Items.FindByValue(Vehiculo.P_Tipo_Vehiculo_ID));
            Cmb_Tipo_Combustible.SelectedIndex = Cmb_Tipo_Combustible.Items.IndexOf(Cmb_Tipo_Combustible.Items.FindByValue(Vehiculo.P_Tipo_Combustible_ID));
            Cmb_Colores.SelectedIndex = Cmb_Colores.Items.IndexOf(Cmb_Colores.Items.FindByValue(Vehiculo.P_Color_ID));
            Cmb_Zonas.SelectedIndex = Cmb_Zonas.Items.IndexOf(Cmb_Zonas.Items.FindByValue(Vehiculo.P_Zona_ID));
            Txt_Numero_Inventario.Text = Vehiculo.P_Numero_Inventario.ToString();
            Txt_Numero_Economico.Text = Vehiculo.P_Numero_Economico.ToString();
            Txt_Placas.Text = Vehiculo.P_Placas;
            Txt_Costo.Text = Vehiculo.P_Costo_Actual.ToString("#,###,###.00");
            Txt_Capacidad_Carga.Text = Vehiculo.P_Capacidad_Carga.ToString();
            Txt_Anio_Fabricacion.Text = Vehiculo.P_Anio_Fabricacion.ToString();
            Txt_Serie_Carroceria.Text = Vehiculo.P_Serie_Carroceria;
            Txt_Numero_Cilindros.Text = Vehiculo.P_Numero_Cilindros.ToString();
            Txt_Fecha_Adquisicion.Text = String.Format("{0:dd/MMM/yyyy}", Vehiculo.P_Fecha_Adquisicion);
            Txt_Kilometraje.Text = Vehiculo.P_Kilometraje.ToString("#,###,###.00");
            Txt_No_Factura.Text = Vehiculo.P_No_Factura_.Trim();
            Hdf_Proveedor_ID.Value = Vehiculo.P_Proveedor_ID;
            if (!String.IsNullOrEmpty(Hdf_Proveedor_ID.Value)) {
                Cls_Cat_Com_Proveedores_Negocio Proveedor_Negocio = new Cls_Cat_Com_Proveedores_Negocio();
                Proveedor_Negocio.P_Proveedor_ID = Hdf_Proveedor_ID.Value;
                DataTable Dt_Proveedor = Proveedor_Negocio.Consulta_Datos_Proveedores();
                if (Dt_Proveedor != null && Dt_Proveedor.Rows.Count > 0) {
                    Txt_Nombre_Proveedor.Text = Dt_Proveedor.Rows[0][Cat_Com_Proveedores.Campo_Nombre].ToString();
                }
            }
            Cmb_Estatus.SelectedIndex = Cmb_Estatus.Items.IndexOf(Cmb_Estatus.Items.FindByValue(Vehiculo.P_Estatus));
            Cmb_Odometro.SelectedIndex = Cmb_Odometro.Items.IndexOf(Cmb_Odometro.Items.FindByValue(Vehiculo.P_Odometro));
            Txt_Observaciones.Text = Vehiculo.P_Observaciones;
            Llenar_Grid_Resguardantes(0, Vehiculo.P_Resguardantes);
            Grid_Resguardantes.Columns[0].Visible = false;
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    #region MPE_Busqueda_Productos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Limpiar_Campos_MPE_Busqueda_Productos
    ///DESCRIPCIÓN: Limpia los filtros para la Busqueda de los productos.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 01/Marzo/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Limpiar_Campos_MPE_Busqueda_Productos()
    {
        Txt_MPE_Clave_Producto.Text = "";
        Txt_MPE_Nombre_Producto.Text = "";
        Cmb_MPE_Marca_Producto.SelectedIndex = 0;
        Cmb_MPE_Modelo_Producto.SelectedIndex = 0;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_MPE_Busqueda_Productos
    ///DESCRIPCIÓN: Hace una consulta a la Base de Datos y llena el Grid con el 
    ///             resultado.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 01/Marzo/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Grid_MPE_Busqueda_Productos(Int32 Pagina)
    {
        try
        {
            Grid_Listado_Productos.SelectedIndex = -1;
            Cls_Ope_Pat_Com_Partes_Vehiculos_Negocio Negocio = new Cls_Ope_Pat_Com_Partes_Vehiculos_Negocio();
            Negocio.P_Clave_Interna = Txt_MPE_Clave_Producto.Text.Trim();
            Negocio.P_Nombre = Txt_MPE_Nombre_Producto.Text.Trim();
            Negocio.P_Marca = (Cmb_MPE_Marca_Producto.SelectedIndex > 0) ? Cmb_MPE_Marca_Producto.SelectedItem.Value : "";
            Negocio.P_Modelo = (Cmb_MPE_Modelo_Producto.SelectedIndex > 0) ? Cmb_MPE_Modelo_Producto.SelectedItem.Value : "";
            Grid_Listado_Productos.DataSource = Negocio.Listar_Productos_Partes();
            Grid_Listado_Productos.PageIndex = Pagina;
            Grid_Listado_Productos.DataBind();
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
    ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Productos
    ///DESCRIPCIÓN: Llena el Grid de los Productos para seleccionarlo.
    ///PARAMETROS: Pagina. Pagina del Grid que se mostrará.     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 04/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Grid_Productos(Int32 Pagina)
    {
        Grid_Listado_MPE_Productos.SelectedIndex = (-1);
        Grid_Listado_MPE_Productos.Columns[1].Visible = true;
        Cls_Cat_Com_Productos_Negocio Productos_Negocio = new Cls_Cat_Com_Productos_Negocio();
        Productos_Negocio.P_Estatus = "ACTIVO";
        Productos_Negocio.P_Tipo = "VEHICULO";
        if (Txt_Nombre_Producto_Buscar.Text.Trim() != "") {
            Productos_Negocio.P_Nombre = Txt_Nombre_Producto_Buscar.Text.Trim();
        }
        DataTable Dt_Productos = Productos_Negocio.Consulta_Datos_Producto();
        Dt_Productos.Columns[Cat_Com_Productos.Campo_Producto_ID].ColumnName = "PRODUCTO_ID";
        Dt_Productos.Columns[Cat_Com_Productos.Campo_Clave].ColumnName = "CLAVE_PRODUCTO";
        Dt_Productos.Columns[Cat_Com_Productos.Campo_Nombre].ColumnName = "NOMBRE_PRODUCTO";
        Dt_Productos.Columns[Cat_Com_Productos.Campo_Descripcion].ColumnName = "DESCRIPCION_PRODUCTO";
        Grid_Listado_MPE_Productos.DataSource = Dt_Productos;
        Grid_Listado_MPE_Productos.PageIndex = Pagina;
        Grid_Listado_MPE_Productos.DataBind();
        Grid_Listado_MPE_Productos.Columns[1].Visible = false;
    }
    
    private void Consultar_Empleados(String Dependencia_ID) { 
        try {
            Session.Remove("Dt_Resguardantes");
            Grid_Resguardantes.DataSource = new DataTable();
            Grid_Resguardantes.DataBind();
            if (Cmb_Dependencias.SelectedIndex > 0) {
                Cls_Ope_Pat_Com_Vehiculos_Negocio Combo = new Cls_Ope_Pat_Com_Vehiculos_Negocio();
                Combo.P_Tipo_DataTable = "EMPLEADOS";
                Combo.P_Dependencia_ID = Dependencia_ID;
                DataTable Tabla = Combo.Consultar_DataTable();
                Llenar_Combo_Empleados(Tabla);
            } else {
                DataTable Tabla = new DataTable();
                Tabla.Columns.Add("EMPLEADO_ID", Type.GetType("System.String"));
                Tabla.Columns.Add("NOMBRE", Type.GetType("System.String"));
                Llenar_Combo_Empleados(Tabla);
            }
        } catch (Exception Ex) {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }
    
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
            Cls_Ope_Pat_Com_Vehiculos_Negocio Negocio = new Cls_Ope_Pat_Com_Vehiculos_Negocio();
            if (Txt_Busqueda_No_Empleado.Text.Trim().Length > 0) { Negocio.P_No_Empleado_Resguardante = Txt_Busqueda_No_Empleado.Text.Trim(); }
            if (Txt_Busqueda_RFC.Text.Trim().Length > 0) { Negocio.P_RFC_Resguardante = Txt_Busqueda_RFC.Text.Trim(); }
            if (Txt_Busqueda_Nombre_Empleado.Text.Trim().Length > 0) { Negocio.P_Nombre_Resguardante = Txt_Busqueda_Nombre_Empleado.Text.Trim(); }
            if (Cmb_Busqueda_Dependencia.SelectedIndex > 0) { Negocio.P_Dependencia_ID = Cmb_Busqueda_Dependencia.SelectedItem.Value; }
            Grid_Busqueda_Empleados_Resguardo.DataSource = Negocio.Consultar_Empleados_Resguardos();
            Grid_Busqueda_Empleados_Resguardo.DataBind();
            Grid_Busqueda_Empleados_Resguardo.Columns[1].Visible = false;
        }

    #endregion
    #endregion

    #region Grids

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Resguardantes_PageIndexChanging
    ///DESCRIPCIÓN: Maneja el Cambio de Pagina de la Resguardantes
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 03/Diciembre/2010 
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

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Partes_PageIndexChanging
    ///DESCRIPCIÓN: Maneja el Cambio de Pagina de Grid_Partes
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 03/Febrero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Partes_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Cls_Ope_Pat_Com_Partes_Vehiculos_Negocio Parte = new Cls_Ope_Pat_Com_Partes_Vehiculos_Negocio();
            Parte.P_Vehiculo_ID = Hdf_Vehiculo_ID.Value;
            Llenar_Grid_Partes(Parte.Consultar_Listado_Partes_Vehiculo(), e.NewPageIndex);
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Partes_SelectedIndexChanged
    ///DESCRIPCIÓN: Maneja el Cambio de Selección de Grid_Partes
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 31/Enero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Partes_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Grid_Partes.SelectedIndex > (-1))
            {

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
    ///NOMBRE DE LA FUNCIÓN: Grid_Resguardos_Partes_PageIndexChanging
    ///DESCRIPCIÓN: Maneja el evento de cambio de página de el Grid de Resguardos de
    ///             la parte.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 03/Febrero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Resguardos_Partes_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            if (Session["Dt_Resguardados_Parte"] != null)
            {
                DataTable Tabla = (DataTable)Session["Dt_Resguardados_Parte"];
                Llenar_Grid_Resguardos_Partes(e.NewPageIndex, Tabla);
                Grid_Resguardos_Partes.SelectedIndex = (-1);
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
    ///NOMBRE DE LA FUNCIÓN: Grid_Listado_Productos_PageIndexChanging
    ///DESCRIPCIÓN: Maneja la paginación del GridView de los Productos
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO:04/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Listado_MPE_Productos_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Llenar_Grid_Productos(e.NewPageIndex);
        }
        catch (Exception Ex)
        {
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
    protected void Grid_Listado_MPE_Productos_SelectedIndexChanged(object sender, EventArgs e) {
        try {
            if (Grid_Listado_MPE_Productos.SelectedIndex > (-1)) {
                String Producto_ID = Grid_Listado_MPE_Productos.SelectedRow.Cells[1].Text.Trim();
                Cls_Cat_Com_Productos_Negocio Producto_Negocio = new Cls_Cat_Com_Productos_Negocio();
                Producto_Negocio.P_Producto_ID = Producto_ID;
                Producto_Negocio.P_Estatus = "ACTIVO";
                DataTable Dt_Producto_Seleccionado = Producto_Negocio.Consulta_Datos_Producto();
                if (Dt_Producto_Seleccionado != null && Dt_Producto_Seleccionado.Rows.Count > 0) {
                    Hdf_Producto_ID.Value = Dt_Producto_Seleccionado.Rows[0][Cat_Com_Productos.Campo_Producto_ID].ToString().Trim();
                    Txt_Nombre_Producto_Donado.Text = Dt_Producto_Seleccionado.Rows[0][Cat_Com_Productos.Campo_Nombre].ToString().Trim();
                    Mpe_Productos_Cabecera.Hide();
                }
                System.Threading.Thread.Sleep(500);
                Grid_Listado_MPE_Productos.SelectedIndex = (-1);
            }
        } catch (Exception Ex) {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Resguardantes_RowDataBound
    ///DESCRIPCIÓN: Maneja el Evento RowDataBound del Grid de Resguardos.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 30/Septiembre/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Resguardantes_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.FindControl("Btn_Ver_Informacion_Resguardo") != null)
                {
                    if (Session["Dt_Resguardantes"] != null)
                    {
                        ImageButton Btn_Informacion = (ImageButton)e.Row.FindControl("Btn_Ver_Informacion_Resguardo");
                        Btn_Informacion.CommandArgument = ((DataTable)Session["Dt_Resguardantes"]).Rows[e.Row.RowIndex]["COMENTARIOS"].ToString();
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = "Verificar.";
            Lbl_Ecabezado_Mensaje.Text = "[Excepción: '" + Ex.Message + "']";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    #region MPE_Busqueda_Productos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Listado_Productos_PageIndexChanging
    ///DESCRIPCIÓN: Maneja el evento de cambio de página de el Grid de Productos de
    ///             la parte.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 04/Marzo/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Listado_Productos_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Llenar_Grid_MPE_Busqueda_Productos(e.NewPageIndex);
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Listado_Productos_SelectedIndexChanged
    ///DESCRIPCIÓN: Maneja el evento de cambio de página de el Grid de Productos de
    ///             la parte.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 04/Marzo/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Listado_Productos_SelectedIndexChanged(object sender, EventArgs e) {
        try  {
            if (Grid_Listado_Productos.SelectedIndex > (-1)) {
                Limpiar_Producto();
                String Producto_ID = Grid_Listado_Productos.SelectedRow.Cells[1].Text.Trim();
                if (Producto_ID.Trim().Length > 0) {
                    Cls_Cat_Com_Productos_Negocio Producto = new Cls_Cat_Com_Productos_Negocio();
                    Producto.P_Producto_ID = Producto_ID;
                    DataTable Dt_Productos = Producto.Consulta_Datos_Producto(); //Consulta los datos del Producto que fue seleccionada por el usuario
                    if (Dt_Productos.Rows.Count > 0) {
                        //Agrega los valores de los campos a los controles correspondientes de la forma
                        foreach (DataRow Registro in Dt_Productos.Rows) {
                            Hdf_Producto_Parte_ID.Value = Producto_ID;
                            Txt_Nombre_Parte.Text = Registro[Cat_Com_Productos.Campo_Nombre].ToString();
                            Cmb_Marca_Parte.SelectedValue = Registro[Cat_Com_Marcas.Campo_Marca_ID].ToString();
                            Cmb_Modelo_Parte.SelectedValue = Registro[Cat_Com_Productos.Campo_Modelo_ID].ToString();
                        }
                    }
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

    #endregion
      
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

    #endregion

    #region Eventos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Ejecutar_Busqueda_Productos_Click
    ///DESCRIPCIÓN: Ejecuta la Busqueda de los productos.
    ///PARAMETROS:     
    ///CREO:        Francisco Antonio Gallardo Castañeda
    ///FECHA_CREO:  12/Septiembre/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Ejecutar_Busqueda_Productos_Click(object sender, ImageClickEventArgs e)  {
        try {
            Llenar_Grid_Productos(0);
            Mpe_Productos_Cabecera.Show();
        } catch (Exception Ex) {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Ejecutar_Busqueda_Productos_Click
    ///DESCRIPCIÓN: Ejecuta la Busqueda de los productos.
    ///PARAMETROS:     
    ///CREO:        Francisco Antonio Gallardo Castañeda
    ///FECHA_CREO:  03/Octubre/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Lanzar_Mpe_Productos_Click(object sender, ImageClickEventArgs e) {
        try  {
            Mpe_Productos_Cabecera.Show();
        } catch (Exception Ex) {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Agregar_Donador_Click
    ///DESCRIPCIÓN: Lanza el Modal para agregar un Nuevo Donador.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 22/Enero/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Agregar_Donador_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Lbl_MPE_Donadores_Cabecera_Error.Text = "";
            Lbl_MPE_Donadores_Mensaje_Error.Text = "";
            Div_MPE_Donadores_Mensaje_Error.Visible = false;
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
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 22/Enero/2010 
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
    ///NOMBRE DE LA FUNCIÓN: Btn_Nuevo_Click
    ///DESCRIPCIÓN: Prepara y da de Alta un Vehículo con uno o mas resguardantes.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 03/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)  {
        try {
            if (Btn_Nuevo.AlternateText.Equals("Nuevo"))  {
                Configuracion_Formulario(false, "VEHICULOS");
                Limpiar_Catalogo_Generales();
                Limpiar_Detalles();
                Session.Remove("Dt_Resguardantes");
                Cmb_Estatus.SelectedIndex = 1;
            } else {
                if (Validar_Componentes_Generales()) {
                    Cls_Ope_Pat_Com_Vehiculos_Negocio Vehiculo = new Cls_Ope_Pat_Com_Vehiculos_Negocio();
                    if (Txt_Producto_ID.Text.Trim().Length > 0)  {
                        Vehiculo.P_Procedencia = "REQUISICION";

                        if (Session["No_Requisicion"] != null)
                            Vehiculo.P_No_Requisicion = Session["No_Requisicion"].ToString().Trim();
                        else
                            Vehiculo.P_No_Requisicion = null;
                    }
                    if (Cmb_Procedencia.SelectedIndex > 0) {
                        Vehiculo.P_Procedencia = Cmb_Procedencia.SelectedItem.Value;
                    }
                    Vehiculo.P_Clase_Activo_ID = Cmb_Clase_Activo.SelectedItem.Value.Trim();
                    Vehiculo.P_Clasificacion_ID = Cmb_Tipo_Activo.SelectedItem.Value.Trim();
                    Vehiculo.P_Marca_ID = Cmb_Marca.SelectedItem.Value;
                    Vehiculo.P_Modelo_ID = Txt_Modelo.Text.Trim();
                    Vehiculo.P_Donador_ID = Hdf_Donador_ID.Value;
                    Vehiculo.P_Producto_ID = Hdf_Producto_ID.Value.Trim();
                    Vehiculo.P_Nombre_Producto = Txt_Nombre_Producto_Donado.Text.Trim();
                    Vehiculo.P_Dependencia_ID = Cmb_Dependencias.SelectedItem.Value.Trim();
                    Vehiculo.P_Tipo_Vehiculo_ID = Cmb_Tipos_Vehiculos.SelectedItem.Value.Trim();
                    Vehiculo.P_Tipo_Combustible_ID = Cmb_Tipo_Combustible.SelectedItem.Value.Trim();
                    Vehiculo.P_Color_ID = Cmb_Colores.SelectedItem.Value.Trim();
                    Vehiculo.P_Zona_ID = Cmb_Zonas.SelectedItem.Value.Trim();
                    Vehiculo.P_Numero_Economico_ = Txt_Numero_Economico.Text.Trim();
                    Vehiculo.P_Placas = Txt_Placas.Text.Trim();
                    Vehiculo.P_Costo_Actual = Convert.ToDouble(Txt_Costo.Text.Trim());
                    //Vehiculo.P_Capacidad_Carga = Txt_Capacidad_Carga.Text.Trim();
                    Vehiculo.P_Anio_Fabricacion = Convert.ToInt32(Txt_Anio_Fabricacion.Text.Trim());
                    Vehiculo.P_Serie_Carroceria = Txt_Serie_Carroceria.Text.Trim();
                    Vehiculo.P_Numero_Cilindros = Convert.ToInt32(Txt_Numero_Cilindros.Text.Trim());
                    Vehiculo.P_Fecha_Adquisicion = Convert.ToDateTime(Txt_Fecha_Adquisicion.Text.Trim());
                    Vehiculo.P_Kilometraje = Convert.ToDouble(Txt_Kilometraje.Text.Trim());
                    Vehiculo.P_Estatus = Cmb_Estatus.SelectedItem.Value.Trim();
                    Vehiculo.P_Odometro = Cmb_Odometro.SelectedItem.Value.Trim();
                    Vehiculo.P_Observaciones = Txt_Observaciones.Text.Trim();
                    if (AFU_Archivo.HasFile) { Vehiculo.P_Archivo = AFU_Archivo.FileName; }
                    Vehiculo.P_Resguardantes = (DataTable)Session["Dt_Resguardantes"];
                    if (Txt_No_Factura.Text.Trim().Length > 0) {
                        Vehiculo.P_No_Factura_ = Txt_No_Factura.Text.Trim();
                    }
                    Vehiculo.P_Proveedor_ID = Hdf_Proveedor_ID.Value;
                    Vehiculo.P_Usuario_Nombre = Cls_Sessiones.Nombre_Empleado;
                    Vehiculo.P_Usuario_ID = Cls_Sessiones.Empleado_ID;
                    Vehiculo.P_Cantidad = 1;
                    Vehiculo.Alta_Vehiculo();
                    Txt_Numero_Inventario.Text = Vehiculo.P_Numero_Inventario.ToString();
                    if (AFU_Archivo.HasFile) {
                        String Ruta = Server.MapPath("../../" + Ope_Pat_Archivos_Bienes.Campo_Ruta_Fisica_Archivos + "/VEHICULOS/" + Vehiculo.P_Vehiculo_ID);
                        if (!Directory.Exists(Ruta)) {
                            Directory.CreateDirectory(Ruta);
                        }
                        String Archivo = Ruta + "/" + Vehiculo.P_Archivo;
                        AFU_Archivo.SaveAs(Archivo);
                    }
                    Vehiculo = Vehiculo.Consultar_Detalles_Vehiculo();
                    Mostrar_Detalles_Vehiculo(Vehiculo);
                    Session.Remove("Dt_Resguardantes");
                    Configuracion_Formulario(true, "VEHICULOS");

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Alta de Vehiculos", "alert('Alta de Vehículo Exitosa');", true);
                    Llenar_DataSet_Resguardos_Vehiculos(Vehiculo);
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
    ///FECHA_CREO: 03/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e) {
        try {
            if (Btn_Salir.AlternateText.Equals("Salir")) {
                Session["Dt_Resguardantes"] = null;
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
            } else {
                Grid_Resguardantes.Enabled = true;
                Configuracion_Formulario(true, "VEHICULOS");
                Limpiar_Catalogo_Generales();
                Limpiar_Catalogo_Generales();
                Limpiar_Detalles();
                Session.Remove("Dt_Resguardantes");
                Btn_Salir.AlternateText = "Salir";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
            }
        } catch (Exception Ex) {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }
    
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Generar_Reporte_Click
    ///DESCRIPCIÓN: Genera el reporte simple.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 04/Junio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Generar_Reporte_Click(object sender, ImageClickEventArgs e) {
        try {
            if (Hdf_Vehiculo_ID.Value.Trim().Length > 0) {
                Cls_Ope_Pat_Com_Vehiculos_Negocio Vehiculo = new Cls_Ope_Pat_Com_Vehiculos_Negocio();
                Vehiculo.P_Vehiculo_ID = Hdf_Vehiculo_ID.Value;
                Vehiculo = Vehiculo.Consultar_Detalles_Vehiculo();
                Llenar_DataSet_Resguardos_Vehiculos(Vehiculo);
            } else {
                Lbl_Ecabezado_Mensaje.Text = "Es necesario.";
                Lbl_Mensaje_Error.Text = "Seleccionar el Vehículo a Generar el Reporte.";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        } catch (Exception Ex) {
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
    ///FECHA_CREO: 03/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Cmb_Dependencias_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            String Dependencia_ID = Cmb_Dependencias.SelectedItem.Value;
            Consultar_Empleados(Dependencia_ID);
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
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 22/Enero/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_MPE_Donador_Aceptar_Click(object sender, ImageClickEventArgs e) {
        try {
            if (Validar_Catalogo_Donador()) {
                Cls_Cat_Pat_Com_Donadores_Negocio Donador = new Cls_Cat_Pat_Com_Donadores_Negocio();
                Donador.P_RFC = Txt_RFC_Donador_MPE.Text.Trim();
                Donador = Donador.Consultar_Datos_Donador();
                if (Donador.P_Donador_ID != null && Donador.P_Donador_ID.Trim().Length > 0)  {
                    Lbl_MPE_Donadores_Cabecera_Error.Text = "Ya existe un donador con ese RFC.";
                    Lbl_MPE_Donadores_Mensaje_Error.Text = "";
                    Div_MPE_Donadores_Mensaje_Error.Visible = true;
                    MPE_Donadores.Show();
                } else {
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
                    Txt_Nombre_Donador.Text = (Txt_Nombre_Donador.Text = Donador.P_Apellido_Paterno + " " + Donador.P_Apellido_Materno + " " + Donador.P_Nombre).Trim();
                    Limpiar_Catalogo_Donadores();
                    Div_MPE_Donadores_Mensaje_Error.Visible = false;
                    MPE_Donadores.Hide();
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




    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Ver_Informacion_Resguardo_Click
    ///DESCRIPCIÓN: Manda Visualizar los Comentarios del Resguardo.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 30/Septiembre/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************
    protected void Btn_Ver_Informacion_Resguardo_Click(object sender, ImageClickEventArgs e)
    {
        ImageButton Btn_Ver_Informacion_Resguardo = (ImageButton)sender;
        String Comentarios = "Sin Comentarios";
        if (Btn_Ver_Informacion_Resguardo.CommandArgument.Trim().Length > 0) { Comentarios = "Comentarios: " + Btn_Ver_Informacion_Resguardo.CommandArgument.Trim(); }
        ScriptManager.RegisterStartupScript(this, this.GetType(), "GACO", "alert('" + Comentarios + "');", true);
    }

    #region Vehículos Detalles

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Agregar_Resguardante_Click
    ///DESCRIPCIÓN: Agrega una nuevo Empleado Resguardante para este Vehículo.
    ///             (No aun en la Base de Datos)
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 03/Diciembre/2010 
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
                    Tabla.Columns.Add("NO_EMPLEADO", Type.GetType("System.String"));
                    Tabla.Columns.Add("NOMBRE_EMPLEADO", Type.GetType("System.String"));
                    Tabla.Columns.Add("COMENTARIOS", Type.GetType("System.String"));
                }
                else
                {
                    Tabla = (DataTable)Session["Dt_Resguardantes"];
                }
            }
            if (!Buscar_Clave_DataTable(Cmb_Empleados.SelectedItem.Value, Tabla, 1)) {
                Cls_Cat_Empleados_Negocios Empleados_Negocio = new Cls_Cat_Empleados_Negocios();
                Empleados_Negocio.P_Empleado_ID = Cmb_Empleados.SelectedItem.Value;
                DataTable Dt_Empleado = Empleados_Negocio.Consulta_Datos_Empleado();
                if (Dt_Empleado != null && Dt_Empleado.Rows.Count > 0) {
                    DataRow Fila = Tabla.NewRow();
                    Fila["BIEN_RESGUARDO_ID"] = HttpUtility.HtmlDecode("");
                    Fila["EMPLEADO_ID"] = Dt_Empleado.Rows[0][Cat_Empleados.Campo_Empleado_ID].ToString().Trim();
                    Fila["NO_EMPLEADO"] = Dt_Empleado.Rows[0][Cat_Empleados.Campo_No_Empleado].ToString().Trim();
                    Fila["NOMBRE_EMPLEADO"] = HttpUtility.HtmlDecode(Cmb_Empleados.SelectedItem.Text);
                    if (Txt_Cometarios.Text.Trim().Length > 255) { Fila["COMENTARIOS"] = HttpUtility.HtmlDecode(Txt_Cometarios.Text.Trim().Substring(0, 254)); }
                    else { Fila["COMENTARIOS"] = HttpUtility.HtmlDecode(Txt_Cometarios.Text); }
                    Tabla.Rows.Add(Fila);
                }
                Llenar_Grid_Resguardantes(Grid_Resguardantes.PageIndex, Tabla);
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
    ///DESCRIPCIÓN: Quita un Empleado resguardante para este Vehículo (No en la Base de datos
    ///             aun).
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 03/Diciembre/2010 
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

    #region Partes-Vehiculo

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Agregar_Partes_Click
    ///DESCRIPCIÓN: Prepara el Formulario para agregarle las partes a los vehiculos.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 01/Febrero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Agregar_Partes_Click(object sender, EventArgs e)
    {
        Limpiar_Partes();
        Configuracion_Formulario(true, "PARTES");
        Btn_Agregar_Partes.Visible = false;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Agregar_Parte_Click
    ///DESCRIPCIÓN: Carga los datos de una parte en un objeto de la Clase de Negocio
    ///             para darlos de alta en la Base de datos.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 01/Febrero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Agregar_Parte_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Validar_Partes())
            {
                Cls_Ope_Pat_Com_Partes_Vehiculos_Negocio Parte = new Cls_Ope_Pat_Com_Partes_Vehiculos_Negocio();
                Parte.P_Producto_ID = Hdf_Producto_Parte_ID.Value;
                Parte.P_Nombre = Txt_Nombre_Parte.Text.Trim();
                Parte.P_Marca = Cmb_Marca_Parte.SelectedItem.Value;
                Parte.P_Modelo = Cmb_Modelo_Parte.SelectedItem.Value;
                Parte.P_Vehiculo_ID = Hdf_Vehiculo_ID.Value;
                Parte.P_Costo = Convert.ToDouble(Txt_Costo_Parte.Text.Trim());
                Parte.P_Material = Cmb_Material_Parte.SelectedItem.Value;
                Parte.P_Color = Cmb_Color_Parte.SelectedItem.Value;
                Parte.P_Numero_Inventario = Txt_Numero_Inventario_Parte.Text.Trim();
                Parte.P_Cantidad = 1;
                Parte.P_Estado = Cmb_Estado_Parte.SelectedItem.Value;
                Parte.P_Estatus = Cmb_Estatus_Parte.SelectedItem.Value;
                Parte.P_Comentarios = Txt_Comentarios_Parte.Text.Trim();
                Parte.P_Fecha_Adquisicion = Convert.ToDateTime(Txt_Fecha_Adquisicion_Parte.Text.Trim());
                Parte.P_Resguardantes = (Session["Dt_Resguardados"] != null) ? (DataTable)Session["Dt_Resguardados"] : new DataTable();
                Parte.P_Usuario_Nombre = Cls_Sessiones.Nombre_Empleado;
                Parte.P_Usuario_ID = Cls_Sessiones.Empleado_ID;
                Parte.Alta_Parte();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Alta de Vehiculos", "alert('Alta de Parte Exitosa');", true);
                Llenar_Grid_Partes(Parte.Consultar_Listado_Partes_Vehiculo(), Grid_Partes.PageIndex);
                Limpiar_Partes();
                Configuracion_Formulario(true, "PARTES");
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
    ///NOMBRE DE LA FUNCIÓN: Btn_Quitar_Parte_Click
    ///DESCRIPCIÓN: Carga el identificador de una parte para ser eliminada en la Base
    ///             de Datos.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 01/Febrero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Quitar_Parte_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Int32 Parte_ID = Convert.ToInt32(Grid_Partes.SelectedRow.Cells[1].Text.Trim());
            Cls_Ope_Pat_Com_Partes_Vehiculos_Negocio Parte = new Cls_Ope_Pat_Com_Partes_Vehiculos_Negocio();
            Parte.P_Parte_ID = Parte_ID;
            Parte.Eliminar_Parte();
            Limpiar_Partes();
            Parte.P_Vehiculo_ID = Hdf_Vehiculo_ID.Value;
            Llenar_Grid_Partes(Parte.Consultar_Listado_Partes_Vehiculo(), Grid_Partes.PageIndex);
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Agregar_Resguardo_Parte_Click
    ///DESCRIPCIÓN: Agrega una nuevo Empleado Resguardante para esta Parte.
    ///             (No aun en la Base de Datos)
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 02/Febrero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Agregar_Resguardo_Parte_Click(object sender, ImageClickEventArgs e)
    {
        if (Validar_Componentes_Resguardos_Partes())
        {
            DataTable Tabla = (DataTable)Grid_Resguardos_Partes.DataSource;
            if (Tabla == null)
            {
                if (Session["Dt_Resguardados_Parte"] == null)
                {
                    Tabla = new DataTable("Resguardos");
                    Tabla.Columns.Add("BIEN_RESGUARDO_ID", Type.GetType("System.String"));
                    Tabla.Columns.Add("EMPLEADO_ID", Type.GetType("System.String"));
                    Tabla.Columns.Add("NOMBRE_EMPLEADO", Type.GetType("System.String"));
                    Tabla.Columns.Add("COMENTARIOS", Type.GetType("System.String"));
                }
                else
                {
                    Tabla = (DataTable)Session["Dt_Resguardados_Parte"];
                }
            }
            if (!Buscar_Clave_DataTable(Cmb_Resguardante_Parte.SelectedItem.Value, Tabla, 1))
            {
                DataRow Fila = Tabla.NewRow();
                Fila["BIEN_RESGUARDO_ID"] = HttpUtility.HtmlDecode("");
                Fila["EMPLEADO_ID"] = HttpUtility.HtmlDecode(Cmb_Resguardante_Parte.SelectedItem.Value);
                Fila["NOMBRE_EMPLEADO"] = HttpUtility.HtmlDecode(Cmb_Resguardante_Parte.SelectedItem.Text);
                Fila["COMENTARIOS"] = HttpUtility.HtmlDecode(Txt_Comentarios_Resguardo_Parte.Text.Trim());
                Tabla.Rows.Add(Fila);
                Session["Dt_Resguardados_Parte"] = Tabla;
                Llenar_Grid_Resguardos_Partes(Grid_Partes.PageIndex, Tabla);
                Cmb_Resguardante_Parte.SelectedIndex = 0;
                Txt_Comentarios_Resguardo_Parte.Text = "";
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
    ///NOMBRE DE LA FUNCIÓN: Btn_Quitar_Resguardo_Parte_Click
    ///DESCRIPCIÓN: Quita un Empleado resguardante para esta Parte (No en la Base de datos
    ///             aun).
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 02/Febrero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Quitar_Resguardo_Parte_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Grid_Resguardos_Partes.Rows.Count > 0 && Grid_Resguardos_Partes.SelectedIndex > (-1))
            {
                Int32 Registro = ((Grid_Resguardos_Partes.PageIndex) * Grid_Resguardos_Partes.PageSize) + (Grid_Resguardos_Partes.SelectedIndex);
                if (Session["Dt_Resguardados_Parte"] != null)
                {
                    DataTable Tabla = (DataTable)Session["Dt_Resguardados_Parte"];
                    Tabla.Rows.RemoveAt(Registro);
                    Session["Dt_Resguardados_Parte"] = Tabla;
                    Grid_Resguardos_Partes.SelectedIndex = (-1);
                    Llenar_Grid_Resguardos_Partes(Grid_Resguardos_Partes.PageIndex, Tabla);
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

    #region MPE_Busqueda_Productos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Lanzar_Buscar_Producto_Click
    ///DESCRIPCIÓN: Lanza el Modal de Busqueda de Productos.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 04/Marzo/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Lanzar_Buscar_Producto_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            MPE_Busqueda_Productos.Show();
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_MPE_Productos_Buscar_Click
    ///DESCRIPCIÓN: Hace la Busqueda de los productos.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 01/Marzo/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_MPE_Productos_Buscar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Llenar_Grid_MPE_Busqueda_Productos(0);
            MPE_Busqueda_Productos.Show();
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_MPE_Productos_Limpiar_Click
    ///DESCRIPCIÓN: Limpia los filtros para la Busqueda de los productos.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 01/Marzo/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_MPE_Productos_Limpiar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Limpiar_Campos_MPE_Busqueda_Productos();
            MPE_Busqueda_Productos.Show();
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    #endregion

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
            Mpe_Productos_Cabecera.Show();
        } catch (Exception Ex) {
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
            Mpe_Proveedores_Cabecera.Show();
        } catch (Exception Ex) {
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













    #endregion


}