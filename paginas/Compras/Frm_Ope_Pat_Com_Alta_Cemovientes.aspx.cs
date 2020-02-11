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
using Presidencia.Constantes;
using Presidencia.Sessiones;
using Presidencia.Control_Patrimonial_Operacion_Cemovientes.Negocio;
using Presidencia.Control_Patrimonial_Catalogo_Donadores.Negocio;
using Presidencia.Almacen_Resguardos.Negocio;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using System.IO;
using Presidencia.Reportes;
using System.Collections.Generic;
using Presidencia.Catalogo_Compras_Productos.Negocio;
using Presidencia.Catalogo_Compras_Proveedores.Negocio;
using Presidencia.Control_Patrimonial_Catalogo_Procedencias.Negocio;
using Presidencia.Empleados.Negocios;
using Presidencia.Control_Patrimonial_Catalogo_Clasificaciones.Negocio;
using Presidencia.Control_Patrimonial_Catalogo_Clases_Activos.Negocio;

public partial class paginas_predial_Frm_Ope_Pat_Com_Alta_Cemovientes : System.Web.UI.Page
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
    ///FECHA_CREO: 17/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Page_Load(object sender, EventArgs e) {
        if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");
        Div_Contenedor_Msj_Error.Visible = false;
        Div_MPE_Donadores_Mensaje_Error.Visible = false;
        if (!IsPostBack)  {
            Configuracion_Formulario(true);
            Llenar_Combos();
            Llenar_Combo_Procedencias();
            Llenar_Grid_Productos(0);
            Llenar_Grid_Proveedores(0);
            Lbl_Inventario_Anterior.Visible = false;
            Txt_Inventario_Anterior.Visible = false;
        }
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
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Empleados
    ///DESCRIPCIÓN: Llena el combo de Empleados.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 17/Diciembre/2010 
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
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combos
    ///DESCRIPCIÓN: Se llenan los Combos Generales Independientes.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 17/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Combos()
    {
        try
        {
            Cls_Ope_Pat_Com_Cemovientes_Negocio Combos = new Cls_Ope_Pat_Com_Cemovientes_Negocio();
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

            //SE LLENA EL COMBO DE TIPOS ALIMENTACION
            Combos.P_Tipo_DataTable = "TIPOS_ALIMENTACION";
            DataTable Tipos_Alimentacion = Combos.Consultar_DataTable();
            DataRow Fila_Tipo_Alimentacion = Tipos_Alimentacion.NewRow();
            Fila_Tipo_Alimentacion["TIPO_ALIMENTACION_ID"] = "SELECCIONE";
            Fila_Tipo_Alimentacion["NOMBRE"] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
            Tipos_Alimentacion.Rows.InsertAt(Fila_Tipo_Alimentacion, 0);
            Cmb_Tipos_Alimentacion.DataSource = Tipos_Alimentacion;
            Cmb_Tipos_Alimentacion.DataValueField = "TIPO_ALIMENTACION_ID";
            Cmb_Tipos_Alimentacion.DataTextField = "NOMBRE";
            Cmb_Tipos_Alimentacion.DataBind();

            //SE LLENA EL COMBO DE TIPOS ADIESTRAMIENTO
            Combos.P_Tipo_DataTable = "TIPOS_ADIESTRAMIENTO";
            DataTable Tipos_Adiestramiento = Combos.Consultar_DataTable();
            DataRow Fila_Tipo_Adiestramiento = Tipos_Adiestramiento.NewRow();
            Fila_Tipo_Adiestramiento["TIPO_ADIESTRAMIENTO_ID"] = "SELECCIONE";
            Fila_Tipo_Adiestramiento["NOMBRE"] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
            Tipos_Adiestramiento.Rows.InsertAt(Fila_Tipo_Adiestramiento, 0);
            Cmb_Tipo_Adiestramiento.DataSource = Tipos_Adiestramiento;
            Cmb_Tipo_Adiestramiento.DataValueField = "TIPO_ADIESTRAMIENTO_ID";
            Cmb_Tipo_Adiestramiento.DataTextField = "NOMBRE";
            Cmb_Tipo_Adiestramiento.DataBind();

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

            //SE LLENA EL COMBO DE RAZAS
            Combos.P_Tipo_DataTable = "RAZAS";
            DataTable Razas = Combos.Consultar_DataTable();
            DataRow Fila_Raza = Razas.NewRow();
            Fila_Raza["RAZA_ID"] = "SELECCIONE";
            Fila_Raza["NOMBRE"] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
            Razas.Rows.InsertAt(Fila_Raza, 0);
            Cmb_Razas.DataSource = Razas;
            Cmb_Razas.DataTextField = "NOMBRE";
            Cmb_Razas.DataValueField = "RAZA_ID";
            Cmb_Razas.DataBind();

            //SE LLENA EL COMBO DE VACUNAS
            Combos.P_Tipo_DataTable = "VACUNAS";
            DataTable Vacunas = Combos.Consultar_DataTable();
            DataRow Fila_Vacuna = Vacunas.NewRow();
            Fila_Vacuna["VACUNA_ID"] = "SELECCIONE";
            Fila_Vacuna["NOMBRE"] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
            Vacunas.Rows.InsertAt(Fila_Vacuna, 0);
            Cmb_Vacunas.DataSource = Vacunas;
            Cmb_Vacunas.DataTextField = "NOMBRE";
            Cmb_Vacunas.DataValueField = "VACUNA_ID";
            Cmb_Vacunas.DataBind();

            //SE LLENA EL COMBO DE FUNCIONES
            Combos.P_Tipo_DataTable = "FUNCIONES";
            DataTable Funciones = Combos.Consultar_DataTable();
            DataRow Fila_Funciones = Funciones.NewRow();
            Fila_Funciones["FUNCION_ID"] = "SELECCIONE";
            Fila_Funciones["NOMBRE"] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
            Funciones.Rows.InsertAt(Fila_Funciones, 0);
            Cmb_Funciones.DataSource = Funciones;
            Cmb_Funciones.DataTextField = "NOMBRE";
            Cmb_Funciones.DataValueField = "FUNCION_ID";
            Cmb_Funciones.DataBind();

            ////SE LLENA EL COMBO DE TIPOS DE CEMOVIENTES
            Combos.P_Tipo_DataTable = "TIPOS_CEMOVIENTES";
            DataTable Tipos_Cemovientes = Combos.Consultar_DataTable();
            DataRow Fila_Tipo_Cemovientes = Tipos_Cemovientes.NewRow();
            Fila_Tipo_Cemovientes["TIPO_CEMOVIENTE_ID"] = "SELECCIONE";
            Fila_Tipo_Cemovientes["NOMBRE"] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
            Tipos_Cemovientes.Rows.InsertAt(Fila_Tipo_Cemovientes, 0);
            Cmb_Tipo_Cemoviente.DataSource = Tipos_Cemovientes;
            Cmb_Tipo_Cemoviente.DataTextField = "NOMBRE";
            Cmb_Tipo_Cemoviente.DataValueField = "TIPO_CEMOVIENTE_ID";
            Cmb_Tipo_Cemoviente.DataBind();

            //SE LLENA EL COMBO DE VETERINARIOS
            Combos.P_Tipo_DataTable = "VETERINARIOS";
            DataTable Veterinarios = Combos.Consultar_DataTable();
            DataRow Fila_Veterinario = Veterinarios.NewRow();
            Fila_Veterinario["VETERINARIO_ID"] = "SELECCIONE";
            Fila_Veterinario["NOMBRE"] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
            Veterinarios.Rows.InsertAt(Fila_Veterinario, 0);
            Cmb_Veterinario_Vacuno.DataSource = Veterinarios;
            Cmb_Veterinario_Vacuno.DataTextField = "NOMBRE";
            Cmb_Veterinario_Vacuno.DataValueField = "VETERINARIO_ID";
            Cmb_Veterinario_Vacuno.DataBind();

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
    ///NOMBRE DE LA FUNCIÓN: Configuracion_Formulario
    ///DESCRIPCIÓN: Carga una configuracion de los controles del Formulario
    ///PROPIEDADES:     
    ///             1. Estatus. Estatus en el que se cargara la configuración de los 
    ///                         controles.
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 17/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Configuracion_Formulario(Boolean Estatus) {
        if (Estatus) {
            Btn_Nuevo.AlternateText = "Nuevo";
            Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
            Btn_Salir.AlternateText = "Salir";
            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
            Btn_Generar_Reporte.Visible = true;
        } else {
            Btn_Nuevo.AlternateText = "Dar de Alta";
            Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
            Btn_Salir.AlternateText = "Cancelar";
            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
            Btn_Generar_Reporte.Visible = false;
        }
        Btn_Buscar_RFC_Donador.Visible = !Estatus;
        Cmb_Tipo_Activo.Enabled = !Estatus;
        Cmb_Clase_Activo.Enabled = !Estatus;
        Btn_Agregar_Donador.Visible = !Estatus;
        Btn_Limpiar_Donador.Visible = !Estatus;
        Btn_Lanzar_Mpe_Productos.Visible = !Estatus;
        Cmb_Tipo_Cemoviente.Enabled = !Estatus;
        Cmb_Sexo.Enabled = !Estatus;
        Cmb_Padre.Enabled = !Estatus;
        Cmb_Madre.Enabled = !Estatus;
        Cmb_Dependencias.Enabled = !Estatus;
        Cmb_Tipos_Alimentacion.Enabled = !Estatus;
        Cmb_Tipo_Adiestramiento.Enabled = !Estatus;
        Cmb_Colores.Enabled = !Estatus;
        Cmb_Razas.Enabled = !Estatus;
        Cmb_Tipos_Ascendencia.Enabled = !Estatus;
        Cmb_Funciones.Enabled = !Estatus;
        Btn_Fecha_Adquisicion.Enabled = !Estatus;
        Btn_Fecha_Nacimiento.Enabled = !Estatus;
        Txt_Nombre.Enabled = !Estatus;
        Txt_Inventario_Anterior.Enabled = !Estatus;
        Txt_Costo.Enabled = !Estatus;
        Txt_Observaciones.Enabled = !Estatus;
        Txt_No_Factura.Enabled = !Estatus;
        Btn_Lanzar_Mpe_Proveedores.Visible = !Estatus;
        AFU_Archivo.Enabled = !Estatus;

        Cmb_Vacunas.Enabled = !Estatus;
        Btn_Fecha_Aplicacion.Enabled = !Estatus;
        Cmb_Veterinario_Vacuno.Enabled = !Estatus;
        Txt_Comentarios_Vacuna.Enabled = !Estatus;
        Btn_Agregar_Vacuna.Visible = !Estatus;
        Btn_Quitar_Vacuna.Visible = !Estatus;

        Cmb_Empleados.Enabled = !Estatus;
        Txt_Comentarios.Enabled = !Estatus;
        Btn_Agregar_Resguardante.Visible = !Estatus;
        Btn_Quitar_Resguardante.Visible = !Estatus;
        Cmb_Procedencia.Enabled = !Estatus;
        Grid_Vacunas.Columns[0].Visible = !Estatus;
        Grid_Resguardantes.Columns[0].Visible = !Estatus;
        Btn_Busqueda_Avanzada_Resguardante.Visible = !Estatus;
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
    ///NOMBRE DE LA FUNCIÓN: Limpiar_Detalles
    ///DESCRIPCIÓN: Limpia los controles de detalles del Formulario
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 17/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Limpiar_Detalles()
    {
        Hdf_Producto_ID.Value = "";
        Hdf_Animal_ID.Value = "";
        Txt_Nombre_Producto_Donado.Text = "";
        Hdf_Donador_ID.Value = "";
        Txt_RFC_Donador.Text = "";
        Txt_Nombre_Donador.Text = "";
        Cmb_Procedencia.SelectedIndex = 0;
        Txt_Nombre_Proveedor.Text = "";
        Hdf_Proveedor_ID.Value = "";
        Txt_No_Factura.Text = "";
        Cmb_Tipo_Activo.SelectedIndex = 0;
        Cmb_Clase_Activo.SelectedIndex = 0;
        Cmb_Tipo_Cemoviente.SelectedIndex = 0;
        Cmb_Tipo_Cemoviente.SelectedIndex = 0;
        Cmb_Dependencias.SelectedIndex = 0;
        Cmb_Tipos_Alimentacion.SelectedIndex = 0;
        Cmb_Tipo_Adiestramiento.SelectedIndex = 0;
        Cmb_Colores.SelectedIndex = 0;
        Cmb_Razas.SelectedIndex = 0;
        Cmb_Tipos_Ascendencia.SelectedIndex = 0;
        Cmb_Funciones.SelectedIndex = 0;
        Txt_Numero_Inventario.Text = "";
        Txt_Nombre.Text = "";
        Txt_Inventario_Anterior.Text = "";
        Txt_Costo.Text = "";
        Txt_Fecha_Nacimiento.Text = "";
        Txt_Fecha_Adquisicion.Text = "";
        Cmb_Sexo.SelectedIndex = 0;
        Cmb_Padre.SelectedIndex = 0;
        Cmb_Madre.SelectedIndex = 0;
        Cmb_Estatus.SelectedIndex = 0;
        Txt_Observaciones.Text = "";
        Limpiar_Detalles_Vacunas();
        Grid_Vacunas.SelectedIndex = -1;
        Grid_Vacunas.DataSource = new DataTable();
        Grid_Vacunas.DataBind();
        Limpiar_Detalles_Resguardos();
        Grid_Resguardantes.SelectedIndex = -1;
        Grid_Resguardantes.DataSource = new DataTable();
        Grid_Resguardantes.DataBind();
        Session.Remove("Dt_Vacunas");
        Session.Remove("Dt_Resguardantes");
        Manejo_Combos_Padres();
        Cargar_Padres_Cemoviente();
        Remover_Sesiones_Control_AsyncFileUpload(AFU_Archivo.ClientID);
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Limpiar_Detalles_Vacunas
    ///DESCRIPCIÓN: Limpia los controles de vacunas del Formulario
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 03/Enero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Limpiar_Detalles_Vacunas()
    {
        Cmb_Vacunas.SelectedIndex = 0;
        Txt_Fecha_Aplicacion.Text = "";
        Cmb_Veterinario_Vacuno.SelectedIndex = 0;
        Txt_Comentarios_Vacuna.Text = "";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Limpiar_Detalles_Resguardos
    ///DESCRIPCIÓN: Limpia los controles de detalles del Formulario
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 03/Enero/2011 
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
    ///NOMBRE DE LA FUNCIÓN: Buscar_Clave_DataTable
    ///DESCRIPCIÓN: Busca una Clave en un DataTable, si la encuentra Retorna 'true'
    ///             en caso contrario 'false'.
    ///PROPIEDADES:  
    ///             1.  Clave.  Clave que se buscara en el DataTable
    ///             2.  Tabla.  Datatable donde se va a buscar la clave.
    ///             3.  Columna.Columna del DataTable donde se va a buscar la clave.
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 17/Diciembre/2010 
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
    ///FECHA_CREO: 17/Diciembre/2010 
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

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Vacunas
    ///DESCRIPCIÓN: Llena la tabla de Vacunas
    ///PROPIEDADES:     
    ///             1.  Pagina. Pagina en la cual se mostrará el Grid_VIew
    ///             2.  Tabla.  Tabla que se va a cargar en el Grid.    
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 05/enero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Grid_Vacunas(Int32 Pagina, DataTable Tabla)
    {
        Grid_Vacunas.Columns[1].Visible = true;
        Grid_Vacunas.Columns[2].Visible = true;
        Grid_Vacunas.DataSource = Tabla;
        Grid_Vacunas.PageIndex = Pagina;
        Grid_Vacunas.DataBind();
        Grid_Vacunas.Columns[1].Visible = false;
        Grid_Vacunas.Columns[2].Visible = false;
        Session["Dt_Vacunas"] = Tabla;
    }

    #endregion

    #region Validaciones

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Componentes_Generales
    ///DESCRIPCIÓN: Hace una validacion de que haya datos en los componentes antes de hacer
    ///             una operación de la pestaña de Generales.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 17/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private Boolean Validar_Componentes_Generales()
    {
        Lbl_Ecabezado_Mensaje.Text = "Es necesario.";
        String Mensaje_Error = "";
        Boolean Validacion = true;
        if (Cmb_Procedencia.SelectedIndex == 0) {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Debe seleccionar una opción del Combo de Procedencia.";
            Validacion = false;
        }
        if (Hdf_Producto_ID.Value != null && Hdf_Producto_ID.Value.Trim().Length == 0) {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Debe seleccionar el producto [Animal].";
            Validacion = false;
        }
        if (Cmb_Dependencias.SelectedIndex == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Debe seleccionar una opción del Combo de Unidad Responsable.";
            Validacion = false;
        }
        if (Cmb_Tipos_Alimentacion.SelectedIndex == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Debe seleccionar una opción del Combo de Tipo de Alimentación.";
            Validacion = false;
        }
        if (Cmb_Tipo_Adiestramiento.SelectedIndex == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Debe seleccionar una opción del Combo de Tipo de Adiestramiento.";
            Validacion = false;
        }
        if (Cmb_Colores.SelectedIndex == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Debe seleccionar una opción del Combo de Color.";
            Validacion = false;
        }
        if (Cmb_Razas.SelectedIndex == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Debe seleccionar una opción del Combo de Raza.";
            Validacion = false;
        }
        if (Cmb_Tipos_Ascendencia.SelectedIndex == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Debe seleccionar una opción del Combo de Tipo de Ascendencia.";
            Validacion = false;
        }
        if (Cmb_Funciones.SelectedIndex == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Debe seleccionar una opción del Combo de Funcion.";
            Validacion = false;
        }
        if (Txt_Fecha_Adquisicion.Text.Trim().Length == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Introducir la Fecha de Adquisición.";
            Validacion = false;
        }
        if (Txt_Nombre.Text.Trim().Length == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Introducir el Nombre.";
            Validacion = false;
        }
        if (Txt_Costo.Text.Trim().Length == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Introducir el Costo Actual.";
            Validacion = false;
        }
        if (Txt_Observaciones.Text.Trim().Length > 0 && Txt_Observaciones.Text.Trim().Length > 255)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Verificar la longitud de las Observaciones (Se pasa por " + (Txt_Observaciones.Text.Trim().Length - 255).ToString() + ").";
            Validacion = false;
        }
        if (Grid_Resguardantes.Rows.Count == 0 || Session["Dt_Resguardantes"] == null)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Debe haber como minimo un empleado para resguardo del Cemoviente.";
            Validacion = false;
        }
        //if (Cmb_Tipos_Ascendencia.SelectedItem.Value.Trim().Equals("GOBIERNO"))
        //{
        //    if (Cmb_Padre.SelectedIndex == 0)
        //    {
        //        if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
        //        Mensaje_Error = Mensaje_Error + "+ Debe seleccionar una opción del Combo de Padre.";
        //        Validacion = false;
        //    }
        //    if (Cmb_Madre.SelectedIndex == 0)
        //    {
        //        if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
        //        Mensaje_Error = Mensaje_Error + "+ Debe seleccionar una opción del Combo de Madre.";
        //        Validacion = false;
        //    }
        //}

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
    ///FECHA_CREO: 17/Diciembre/2010 
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
    ///NOMBRE DE LA FUNCIÓN: Validar_Componentes_Vacuanas
    ///DESCRIPCIÓN: Hace una validacion de que haya datos en los componentes antes de hacer
    ///             una operación de Vacunas.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 05/Enero/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private Boolean Validar_Componentes_Vacunas()
    {
        Lbl_Ecabezado_Mensaje.Text = "Es necesario.";
        String Mensaje_Error = "";
        Boolean Validacion = true;
        if (Cmb_Vacunas.SelectedIndex == 0)
        {
            Mensaje_Error = Mensaje_Error + "+ Seleccionar la Vacuna.";
            Validacion = false;
        }
        if (Txt_Fecha_Aplicacion.Text.Trim().Length == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Introducir la Fecha de Aplicación de la Vacuna.";
            Validacion = false;
        }
        if (Cmb_Veterinario_Vacuno.SelectedIndex == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Seleccionar a quien Aplicó la Vacuna.";
            Validacion = false;
        }
        if (Txt_Comentarios_Vacuna.Text.Trim().Length > 255)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Los Comentarios de la vacuna deben ser de Máximo 255 Caracteres ( Sobrepasa el límite por " + (Txt_Comentarios_Vacuna.Text.Trim().Length - 255) + ").";
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

    #endregion

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Manejo_Combo_Padres
    ///DESCRIPCIÓN: Maneja la habilitación y deshabilitación de los combos padres
    ///             dependiendo del combo de Tipo de Ascendencia.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 05/Enero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Manejo_Combos_Padres()  {
        if (Cmb_Tipos_Ascendencia.SelectedIndex == 2 && Cmb_Tipos_Ascendencia.Enabled) {
            Cmb_Padre.Enabled = true;
            Cmb_Madre.Enabled = true;
        } else {
            Cmb_Padre.SelectedIndex = 0;
            Cmb_Madre.SelectedIndex = 0;
            Cmb_Padre.Enabled = false;
            Cmb_Madre.Enabled = false;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cargar_Padres_Cemoviente
    ///DESCRIPCIÓN: Maneja el llenado de los combos de Padres del cemoviente.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 05/Enero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Cargar_Padres_Cemoviente()
    {
        try
        {
            DataTable Padres = new DataTable();
            Cls_Ope_Pat_Com_Cemovientes_Negocio Cemovientes_Negocio = new Cls_Ope_Pat_Com_Cemovientes_Negocio();
            Cemovientes_Negocio.P_Tipo_DataTable = "CEMOVIENTES_PADRES";
            Cemovientes_Negocio.P_Tipo_Cemoviente_ID = (Cmb_Tipo_Cemoviente.SelectedIndex > 0) ? Cmb_Tipo_Cemoviente.SelectedItem.Value.Trim() : "";
            Cemovientes_Negocio.P_Sexo = "MACHO";
            Padres = Cemovientes_Negocio.Consultar_DataTable();
            DataRow Fila = Padres.NewRow();
            Fila["CEMOVIENTE_ID"] = HttpUtility.HtmlDecode("SELECCIONE");
            Fila["NOMBRE"] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
            Padres.Rows.InsertAt(Fila, 0);
            Cmb_Padre.DataSource = Padres;
            Cmb_Padre.DataTextField = "NOMBRE";
            Cmb_Padre.DataValueField = "CEMOVIENTE_ID";
            Cmb_Padre.DataBind();
            DataTable Madres = new DataTable();
            Cemovientes_Negocio.P_Sexo = "HEMBRA";
            Madres = Cemovientes_Negocio.Consultar_DataTable();
            Fila = Madres.NewRow();
            Fila["CEMOVIENTE_ID"] = HttpUtility.HtmlDecode("SELECCIONE");
            Fila["NOMBRE"] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
            Madres.Rows.InsertAt(Fila, 0);
            Cmb_Madre.DataSource = Madres;
            Cmb_Madre.DataTextField = "NOMBRE";
            Cmb_Madre.DataValueField = "CEMOVIENTE_ID";
            Cmb_Madre.DataBind();
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_DataSet_Resguardos_Cemovientes
    ///DESCRIPCIÓN: Llena el dataSet "Data_Set_Resguardos_Vehiculos" con las personas a las que se les asigno el
    ///vehiculo, sus detalles generales y especificos, para que con estos datos se genere el reporte.
    ///PARAMETROS:  
    ///CREO: Salvador Hernández Ramírez
    ///FECHA_CREO: 23/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Llenar_DataSet_Resguardos_Cemovientes(Cls_Ope_Pat_Com_Cemovientes_Negocio Id_Cemoviente)
    {
        String Formato = "PDF";
        Cls_Alm_Com_Resguardos_Negocio Consulta_Resguardos_Cemovientes = new Cls_Alm_Com_Resguardos_Negocio();
        DataSet Ds_Consulta_Resguardos_Cemovientes = new DataSet();
        Ds_Consulta_Resguardos_Cemovientes = Consulta_Resguardos_Cemovientes.Consulta_Resguardos_Cemovientes(Id_Cemoviente);
        Ds_Alm_Com_Resguardos_Cemovientes DS_Reporte_Resguardos_Cemovientes = new Ds_Alm_Com_Resguardos_Cemovientes();
        Generar_Reporte(Ds_Consulta_Resguardos_Cemovientes, DS_Reporte_Resguardos_Cemovientes, Formato);
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Generar_Reporte
    ///DESCRIPCIÓN:          Cargara el data Set fisico con el cual se genera el Reporte especificado
    ///PARAMETROS:           1.-Ds_Consulta_Resguardos_Cemovientes.- Contiene la informacion de la consulta a la base de datos
    ///                      2.-Ds_Reporte, Objeto que contiene la instancia del Data Set fisico del Reporte a generar
    ///                      3.-Nombre_Reporte, contiene el nombre del Reporte a mostrar en pantalla
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           15/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Generar_Reporte(DataSet Ds_Consulta_Resguardos_Cemovientes, DataSet Ds_Reporte, String Formato)
    {
        String Ruta_Reporte_Crystal = "";
        String Nombre_Reporte_Generar = "";
        DataRow Renglon;

        try
        {
            Renglon = Ds_Consulta_Resguardos_Cemovientes.Tables[0].Rows[0];

            String Cantidad = Ds_Consulta_Resguardos_Cemovientes.Tables[0].Rows[0]["CANTIDAD"].ToString();
            String Costo = Ds_Consulta_Resguardos_Cemovientes.Tables[0].Rows[0]["COSTO_ACTUAL"].ToString();
            Double Resultado = (Convert.ToDouble(Cantidad)) * (Convert.ToDouble(Costo));
            Ds_Reporte.Tables[0].ImportRow(Renglon);
            Ds_Reporte.Tables[0].Rows[0].SetField("COSTO_TOTAL", Resultado);

            //En caso de que tenga Padres de Gobierno
            if (!String.IsNullOrEmpty(Ds_Consulta_Resguardos_Cemovientes.Tables[0].Rows[0]["PADRE"].ToString())) {
                Cls_Ope_Pat_Com_Cemovientes_Negocio Animal_Tmp = new Cls_Ope_Pat_Com_Cemovientes_Negocio();
                Animal_Tmp.P_Cemoviente_ID = Ds_Consulta_Resguardos_Cemovientes.Tables[0].Rows[0]["PADRE"].ToString().Trim();
                Animal_Tmp = Animal_Tmp.Consultar_Detalles_Cemoviente();
                Ds_Reporte.Tables[0].Rows[0].SetField("PADRE", Animal_Tmp.P_Nombre);
            }
            if (!String.IsNullOrEmpty(Ds_Consulta_Resguardos_Cemovientes.Tables[0].Rows[0]["MADRE"].ToString())) {
                Cls_Ope_Pat_Com_Cemovientes_Negocio Animal_Tmp = new Cls_Ope_Pat_Com_Cemovientes_Negocio();
                Animal_Tmp.P_Cemoviente_ID = Ds_Consulta_Resguardos_Cemovientes.Tables[0].Rows[0]["MADRE"].ToString().Trim();
                Animal_Tmp = Animal_Tmp.Consultar_Detalles_Cemoviente();
                Ds_Reporte.Tables[0].Rows[0].SetField("MADRE", Animal_Tmp.P_Nombre);
            }

            for (int Cont_Elementos = 0; Cont_Elementos < Ds_Consulta_Resguardos_Cemovientes.Tables[0].Rows.Count; Cont_Elementos++)
            {
                Renglon = Ds_Consulta_Resguardos_Cemovientes.Tables[0].Rows[Cont_Elementos]; //Instanciar renglon e importarlo
                Ds_Reporte.Tables[1].ImportRow(Renglon);

                String Nombre_E = Ds_Consulta_Resguardos_Cemovientes.Tables[0].Rows[Cont_Elementos]["NOMBRE_E"].ToString();
                String Apellido_Paterno_E = Ds_Consulta_Resguardos_Cemovientes.Tables[0].Rows[Cont_Elementos]["APELLIDO_PATERNO_E"].ToString();
                String Apellido_Materno_E = Ds_Consulta_Resguardos_Cemovientes.Tables[0].Rows[Cont_Elementos]["APELLIDO_MATERNO_E"].ToString();
                String RFC_E = Ds_Consulta_Resguardos_Cemovientes.Tables[0].Rows[Cont_Elementos]["RFC_E"].ToString();
                String Resguardante = Nombre_E + " " + Apellido_Paterno_E + " " + Apellido_Materno_E + " " + "(" + RFC_E + ")";
                Ds_Reporte.Tables[1].Rows[Cont_Elementos].SetField("RESGUARDANTES", Resguardante);
            }

            // Ruta donde se encuentra el reporte Crystal
            Ruta_Reporte_Crystal = "../Rpt/Compras/Rpt_Alm_Com_Resguardos_Cemovientes.rpt";

            // Se crea el nombre del reporte
            String Nombre_Reporte = "Rpt_Resguardo_Animales_" + Cls_Sessiones.No_Empleado + "_" + Convert.ToString(DateTime.Now.ToString("yyyy'-'MM'-'dd'_t'HH'-'mm'-'ss"));

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
        Productos_Negocio.P_Tipo = "CEMOVIENTE";
        Productos_Negocio.P_Estatus = "ACTIVO";
        if (Txt_Nombre_Producto_Buscar.Text.Trim() != "") {
            Productos_Negocio.P_Nombre = Txt_Nombre_Producto_Buscar.Text.Trim();
        }
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
    private void Llenar_Grid_Busqueda_Empleados_Resguardo()
    {
        Grid_Busqueda_Empleados_Resguardo.SelectedIndex = (-1);
        Grid_Busqueda_Empleados_Resguardo.Columns[1].Visible = true;
        Cls_Ope_Pat_Com_Cemovientes_Negocio Negocio = new Cls_Ope_Pat_Com_Cemovientes_Negocio();
        if (Txt_Busqueda_No_Empleado.Text.Trim().Length > 0) { Negocio.P_No_Empleado_Resguardante = Txt_Busqueda_No_Empleado.Text.Trim(); }
        if (Txt_Busqueda_RFC.Text.Trim().Length > 0) { Negocio.P_RFC_Resguardante = Txt_Busqueda_RFC.Text.Trim(); }
        if (Txt_Busqueda_Nombre_Empleado.Text.Trim().Length > 0) { Negocio.P_Nombre_Resguardante = Txt_Busqueda_Nombre_Empleado.Text.Trim(); }
        if (Cmb_Busqueda_Dependencia.SelectedIndex > 0) { Negocio.P_Dependencia_ID = Cmb_Busqueda_Dependencia.SelectedItem.Value; }
        Grid_Busqueda_Empleados_Resguardo.DataSource = Negocio.Consultar_Empleados_Resguardos();
        Grid_Busqueda_Empleados_Resguardo.DataBind();
        Grid_Busqueda_Empleados_Resguardo.Columns[1].Visible = false;
    }

    #endregion

    private void Consultar_Empleados(String Dependencia_ID)
    {
        Session.Remove("Dt_Resguardantes");
        Grid_Resguardantes.DataSource = new DataTable();
        Grid_Resguardantes.DataBind();
        if (Cmb_Dependencias.SelectedIndex > 0)
        {
            Cls_Ope_Pat_Com_Cemovientes_Negocio Combo = new Cls_Ope_Pat_Com_Cemovientes_Negocio();
            Combo.P_Tipo_DataTable = "EMPLEADOS";
            Combo.P_Dependencia_ID = Dependencia_ID;
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

    #endregion

    #region Grids

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Resguardantes_PageIndexChanging
    ///DESCRIPCIÓN: Maneja el Cambio de Pagina de la Resguardantes
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 17/Diciembre/2010 
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
    ///NOMBRE DE LA FUNCIÓN: Grid_Vacunas_PageIndexChanging
    ///DESCRIPCIÓN: Maneja el Cambio de Pagina de la Vacunas
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 05/Enero/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Vacunas_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            if (Session["Dt_Vacunas"] != null)
            {
                DataTable Tabla = (DataTable)Session["Dt_Vacunas"];
                Llenar_Grid_Vacunas(e.NewPageIndex, Tabla);
                Grid_Vacunas.SelectedIndex = (-1);
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
    protected void Grid_Listado_Productos_PageIndexChanging(object sender, GridViewPageEventArgs e) {
        try {
            Llenar_Grid_Productos(e.NewPageIndex);
            Mpe_Productos_Cabecera.Show();
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
                }
                System.Threading.Thread.Sleep(500);
                Grid_Listado_Productos.SelectedIndex = (-1);
                Mpe_Productos_Cabecera.Hide();
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



    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Vacunas_RowDataBound
    ///DESCRIPCIÓN: Maneja el Evento RowDataBound del Grid de Resguardos.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 04/Octubre/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Vacunas_RowDataBound(object sender, GridViewRowEventArgs e) {
        try {
            if (e.Row.RowType == DataControlRowType.DataRow) {
                if (e.Row.FindControl("Btn_Ver_Informacion_Vacuna") != null)
                {
                    if (Session["Dt_Vacunas"] != null)  {
                        ImageButton Btn_Informacion = (ImageButton)e.Row.FindControl("Btn_Ver_Informacion_Vacuna");
                        Btn_Informacion.CommandArgument = ((DataTable)Session["Dt_Vacunas"]).Rows[e.Row.RowIndex]["COMENTARIOS"].ToString();
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
    protected void Btn_Ejecutar_Busqueda_Productos_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Llenar_Grid_Productos(0);
            Mpe_Productos_Cabecera.Show();
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
    ///DESCRIPCIÓN: Prepara y da de Alta un Cemoviente con uno o mas resguardantes.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 17/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e) {
        try {
            if (Btn_Nuevo.AlternateText.Equals("Nuevo")) {
                Configuracion_Formulario(false);
                Limpiar_Detalles();
                Session.Remove("Dt_Resguardantes");
                Cmb_Estatus.SelectedIndex = 1;
            } else {
                if (Validar_Componentes_Generales()) {
                    Cls_Ope_Pat_Com_Cemovientes_Negocio Cemoviente = new Cls_Ope_Pat_Com_Cemovientes_Negocio();
                    Cemoviente.P_Producto_ID = Hdf_Producto_ID.Value;
                    if (Cmb_Procedencia.SelectedIndex > 0) { 
                        Cemoviente.P_Procedencia = Cmb_Procedencia.SelectedItem.Value;
                    }
                    Cemoviente.P_Clase_Activo_ID = Cmb_Clase_Activo.SelectedItem.Value.Trim();
                    Cemoviente.P_Clasificacion_ID = Cmb_Tipo_Activo.SelectedItem.Value.Trim();
                    Cemoviente.P_Donador_ID = Hdf_Donador_ID.Value;
                    Cemoviente.P_Dependencia_ID = Cmb_Dependencias.SelectedItem.Value.Trim();
                    Cemoviente.P_Cantidad = 1;
                    Cemoviente.P_Tipo_Alimentacion_ID = Cmb_Tipos_Alimentacion.SelectedItem.Value.Trim();
                    Cemoviente.P_Tipo_Adiestramiento_ID = Cmb_Tipo_Adiestramiento.SelectedItem.Value.Trim();
                    Cemoviente.P_Raza_ID = Cmb_Razas.SelectedItem.Value.Trim();
                    Cemoviente.P_Tipo_Ascendencia = Cmb_Tipos_Ascendencia.SelectedItem.Value.Trim();
                    if (Cmb_Padre.SelectedIndex > 0) {
                        Cemoviente.P_Padre_ID = Cmb_Padre.SelectedItem.Value;
                    }
                    if (Cmb_Madre.SelectedIndex > 0) {
                        Cemoviente.P_Madre_ID = Cmb_Madre.SelectedItem.Value;
                    }
                    Cemoviente.P_Tipo_Cemoviente_ID = Cmb_Tipo_Cemoviente.SelectedItem.Value;
                    Cemoviente.P_Sexo = Cmb_Sexo.SelectedItem.Value;
                    Cemoviente.P_Funcion_ID = Cmb_Funciones.SelectedItem.Value.Trim();
                    Cemoviente.P_Color_ID = Cmb_Colores.SelectedItem.Value.Trim();
                    Cemoviente.P_Nombre = Txt_Nombre.Text.Trim();
                    Cemoviente.P_Fecha_Nacimiento = Convert.ToDateTime(Txt_Fecha_Nacimiento.Text.Trim());
                    Cemoviente.P_Fecha_Adquisicion = Convert.ToDateTime(Txt_Fecha_Adquisicion.Text.Trim());
                    Cemoviente.P_Estatus = Cmb_Estatus.SelectedItem.Value.Trim();
                    Cemoviente.P_Costo_Actual = Convert.ToDouble(Txt_Costo.Text.Trim());
                    if (Txt_No_Factura.Text.Trim().Length > 0) {
                        Cemoviente.P_No_Factura = Txt_No_Factura.Text.Trim();
                    }
                    Cemoviente.P_Proveedor_ID = Hdf_Proveedor_ID.Value.Trim();
                    Cemoviente.P_Observaciones = Txt_Observaciones.Text.Trim();
                    if (AFU_Archivo.HasFile) { Cemoviente.P_Archivo = AFU_Archivo.FileName; }
                    Cemoviente.P_Resguardantes = (Session["Dt_Resguardantes"] != null) ? (DataTable)Session["Dt_Resguardantes"] : new DataTable();
                    Cemoviente.P_Dt_Vacunas = (Session["Dt_Vacunas"] != null) ? (DataTable)Session["Dt_Vacunas"] : new DataTable();
                    Cemoviente.P_Usuario_Nombre = Cls_Sessiones.Nombre_Empleado;
                    Cemoviente.P_Usuario_ID = Cls_Sessiones.Empleado_ID;
                    Cemoviente.Alta_Cemoviente();
                    Txt_Numero_Inventario.Text = Cemoviente.P_Numero_Inventario.ToString();
                    if (AFU_Archivo.HasFile) {
                        String Ruta = Server.MapPath("../../" + Ope_Pat_Archivos_Bienes.Campo_Ruta_Fisica_Archivos + "/CEMOVIENTES/" + Cemoviente.P_Cemoviente_ID);
                        if (!Directory.Exists(Ruta))  {
                            Directory.CreateDirectory(Ruta);
                        }
                        String Archivo = Ruta + "/" + Cemoviente.P_Archivo;
                        AFU_Archivo.SaveAs(Archivo);
                    }
                    Session.Remove("Dt_Resguardantes");
                    Hdf_Animal_ID.Value = Cemoviente.P_Cemoviente_ID.Trim();
                    Configuracion_Formulario(true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Alta de Animal", "alert('Alta de Animal Exitosa');", true);
                    Llenar_DataSet_Resguardos_Cemovientes(Cemoviente);
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
    ///FECHA_CREO: 17/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e) {
        try {
            if (Btn_Salir.AlternateText.Equals("Salir")) {
                Session.Remove("Dt_Resguardantes");
                Session.Remove("Dt_Vacunas");
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
            } else {
                Grid_Resguardantes.Enabled = true;
                Configuracion_Formulario(true);
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
    ///NOMBRE DE LA FUNCIÓN: Cmb_Dependencias_SelectedIndexChanged
    ///DESCRIPCIÓN: Maneja el evento de cambio de Selección del Combo de Dependencias
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 17/Diciembre/2010 
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
    ///NOMBRE DE LA FUNCIÓN: Cmb_Tipo_Cemoviente_SelectedIndexChanged
    ///DESCRIPCIÓN: Maneja el evento de cambio de selección en el combo de Tipo de 
    ///             Cemoviente.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 05/Enero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Cmb_Tipo_Cemoviente_SelectedIndexChanged(object sender, EventArgs e)
    {
        Cargar_Padres_Cemoviente();
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cmb_Tipos_Ascendencia_SelectedIndexChanged
    ///DESCRIPCIÓN: Maneja el evento de cambio de selección en el combo de Tipo de 
    ///             Ascendencia.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 05/Enero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Cmb_Tipos_Ascendencia_SelectedIndexChanged(object sender, EventArgs e)
    {
        Manejo_Combos_Padres();
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
                    Txt_Nombre_Donador.Text = (Donador.P_Apellido_Paterno + " " + Donador.P_Apellido_Materno + " " + Donador.P_Nombre).Trim();
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
    }//

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
    ///NOMBRE DE LA FUNCIÓN: Btn_Lanzar_Mpe_Productos_Click
    ///DESCRIPCIÓN: Lanza los MPE de Productos
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 09/Agosto/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Lanzar_Mpe_Productos_Click(object sender, ImageClickEventArgs e) {
        try {
            Mpe_Productos_Cabecera.Show();
        } catch (Exception Ex) {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
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
            if (Hdf_Animal_ID.Value.Trim().Length > 0) {
                Cls_Ope_Pat_Com_Cemovientes_Negocio Animal = new Cls_Ope_Pat_Com_Cemovientes_Negocio();
                Animal.P_Cemoviente_ID = Hdf_Animal_ID.Value;
                Animal = Animal.Consultar_Detalles_Cemoviente();
                Llenar_DataSet_Resguardos_Cemovientes(Animal);
            } else {
                Lbl_Ecabezado_Mensaje.Text = "Es necesario.";
                Lbl_Mensaje_Error.Text = "Tener un Animal para Reimprimir el Resguardo";
                Div_Contenedor_Msj_Error.Visible = true;
            }
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


    #region Cemovientes Resguardos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Agregar_Resguardante_Click
    ///DESCRIPCIÓN: Agrega una nuevo Empleado Resguardante para este Cemoviente.
    ///             (No aun en la Base de Datos)
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 03/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Agregar_Resguardante_Click(object sender, ImageClickEventArgs e) {
        if (Validar_Componentes_Resguardos()) {
            DataTable Tabla = (DataTable)Grid_Resguardantes.DataSource;
            if (Tabla == null) {
                if (Session["Dt_Resguardantes"] == null) {
                    Tabla = new DataTable("Resguardos");
                    Tabla.Columns.Add("BIEN_RESGUARDO_ID", Type.GetType("System.String"));
                    Tabla.Columns.Add("EMPLEADO_ID", Type.GetType("System.String"));
                    Tabla.Columns.Add("NO_EMPLEADO", Type.GetType("System.String"));
                    Tabla.Columns.Add("NOMBRE_EMPLEADO", Type.GetType("System.String"));
                    Tabla.Columns.Add("COMENTARIOS", Type.GetType("System.String"));
                } else {
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
                    if (Txt_Comentarios.Text.Trim().Length > 255) { Fila["COMENTARIOS"] = HttpUtility.HtmlDecode(Txt_Comentarios.Text.Trim().Substring(0, 254)); }
                    else { Fila["COMENTARIOS"] = HttpUtility.HtmlDecode(Txt_Comentarios.Text); }
                    Tabla.Rows.Add(Fila);
                }
                Llenar_Grid_Resguardantes(Grid_Resguardantes.PageIndex, Tabla);
                Grid_Resguardantes.SelectedIndex = (-1);
                Cmb_Empleados.SelectedIndex = 0;
                Txt_Comentarios.Text = "";
            } else {
                Lbl_Ecabezado_Mensaje.Text = "El Empleado ya esta Agregado.";
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Quitar_Resguardante_Click
    ///DESCRIPCIÓN: Quita un Empleado resguardante para este Cemoviente (No en la Base de datos
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
    }//

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Ver_Informacion_Vacuna_Click
    ///DESCRIPCIÓN: Manda Visualizar los Comentarios del Resguardo.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 04/Octubre/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************
    protected void Btn_Ver_Informacion_Vacuna_Click(object sender, ImageClickEventArgs e) {
        ImageButton Btn_Ver_Informacion_Resguardo = (ImageButton)sender;
        String Comentarios = "Sin Comentarios";
        if (Btn_Ver_Informacion_Resguardo.CommandArgument.Trim().Length > 0) { Comentarios = "Comentarios: " + Btn_Ver_Informacion_Resguardo.CommandArgument.Trim(); }
        ScriptManager.RegisterStartupScript(this, this.GetType(), "GACO", "alert('" + Comentarios + "');", true);
    }
    #endregion

    #region Cemovientes Vacunas

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Agregar_Vacuna_Click
    ///DESCRIPCIÓN: Agrega una nueva Vacuna para este Cemoviente.
    ///             (No aun en la Base de Datos)
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 05/Enero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Agregar_Vacuna_Click(object sender, ImageClickEventArgs e)
    {
        if (Validar_Componentes_Vacunas())
        {
            DataTable Tabla = (DataTable)Grid_Resguardantes.DataSource;
            if (Tabla == null)
            {
                if (Session["Dt_Vacunas"] == null)
                {
                    Tabla = new DataTable("Vacunas");
                    Tabla.Columns.Add("VACUNA_CEMOVIENTE_ID", Type.GetType("System.Int32"));
                    Tabla.Columns.Add("VACUNA_ID", Type.GetType("System.String"));
                    Tabla.Columns.Add("VACUNA_NOMBRE", Type.GetType("System.String"));
                    Tabla.Columns.Add("VETERINARIO_ID", Type.GetType("System.String"));
                    Tabla.Columns.Add("VETERINARIO_NOMBRE", Type.GetType("System.String"));
                    Tabla.Columns.Add("FECHA_APLICACION", Type.GetType("System.DateTime"));
                    Tabla.Columns.Add("COMENTARIOS", Type.GetType("System.String"));
                    Tabla.Columns.Add("ESTATUS", Type.GetType("System.String"));
                    Tabla.Columns.Add("MOTIVO_CANCELACION", Type.GetType("System.String"));
                }
                else
                {
                    Tabla = (DataTable)Session["Dt_Vacunas"];
                }
            }
            DataRow Fila = Tabla.NewRow();
            Fila["VACUNA_CEMOVIENTE_ID"] = 0;
            Fila["VACUNA_ID"] = HttpUtility.HtmlDecode(Cmb_Vacunas.SelectedItem.Value);
            Fila["VACUNA_NOMBRE"] = HttpUtility.HtmlDecode(Cmb_Vacunas.SelectedItem.Text);
            Fila["VETERINARIO_ID"] = HttpUtility.HtmlDecode(Cmb_Veterinario_Vacuno.SelectedItem.Value);
            Fila["VETERINARIO_NOMBRE"] = HttpUtility.HtmlDecode(Cmb_Veterinario_Vacuno.SelectedItem.Text);
            Fila["FECHA_APLICACION"] = Convert.ToDateTime(Txt_Fecha_Aplicacion.Text.Trim());
            Fila["COMENTARIOS"] = HttpUtility.HtmlDecode(Txt_Comentarios_Vacuna.Text.Trim());
            Fila["ESTATUS"] = HttpUtility.HtmlDecode("NUEVA");
            Fila["MOTIVO_CANCELACION"] = HttpUtility.HtmlDecode("");
            Tabla.Rows.Add(Fila);
            Llenar_Grid_Vacunas(Grid_Vacunas.PageIndex, Tabla);
            Cmb_Vacunas.SelectedIndex = (-1);
            Txt_Fecha_Aplicacion.Text = "";
            Cmb_Veterinario_Vacuno.SelectedIndex = (-1);
            Txt_Comentarios_Vacuna.Text = "";
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Quitar_Vacuna_Click
    ///DESCRIPCIÓN: Quita una vacuna para este Cemoviente (No en la Base de datos
    ///             aun).
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 05/Enero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Quitar_Vacuna_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Grid_Vacunas.Rows.Count > 0 && Grid_Vacunas.SelectedIndex > (-1))
            {
                Int32 Registro = ((Grid_Vacunas.PageIndex) * Grid_Vacunas.PageSize) + (Grid_Vacunas.SelectedIndex);
                if (Session["Dt_Vacunas"] != null)
                {
                    DataTable Tabla = (DataTable)Session["Dt_Vacunas"];
                    Tabla.Rows.RemoveAt(Registro);
                    Session["Dt_Vacunas"] = Tabla;
                    Grid_Vacunas.SelectedIndex = (-1);
                    Llenar_Grid_Vacunas(Grid_Vacunas.PageIndex, Tabla);
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

    #endregion




}