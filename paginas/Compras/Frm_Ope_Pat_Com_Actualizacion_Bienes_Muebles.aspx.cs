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
using Presidencia.Control_Patrimonial_Operacion_Bienes_Muebles.Negocio;
using Presidencia.Control_Patrimonial_Operacion_Vehiculos.Negocio;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using System.IO;
using Presidencia.Almacen_Resguardos.Negocio;
using Presidencia.Reportes;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using System.Collections.Generic;
using Presidencia.Catalogo_Compras_Proveedores.Negocio;
using Presidencia.Control_Patrimonial_Catalogo_Procedencias.Negocio;
using Presidencia.Empleados.Negocios;
using Presidencia.Catalogo_Compras_Marcas.Negocio;
using Presidencia.Control_Patrimonial_Catalogo_Tipos_Vehiculo.Negocio;
using Presidencia.Control_Patrimonial_Catalogo_Colores.Negocio;
using Presidencia.Control_Patrimonial_Catalogo_Clasificaciones.Negocio;
using Presidencia.Control_Patrimonial_Catalogo_Clases_Activos.Negocio;

public partial class paginas_Compras_Frm_Ope_Pat_Com_Actualizacion_Bienes_Muebles : System.Web.UI.Page {

    #region Variables Internas

        Cls_Alm_Com_Resguardos_Negocio Consulta_Resguardos_Negocio = new Cls_Alm_Com_Resguardos_Negocio();
        Cls_Ope_Pat_Com_Bienes_Muebles_Negocio Combo = new Cls_Ope_Pat_Com_Bienes_Muebles_Negocio();
        //String Fecha_Adquisicion = "";

    #endregion

    #region Page_Load

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Page_Load
        ///DESCRIPCIÓN: Metodo que se carga cada que ocurre un PostBack de la Página
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 01/Diciembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************  
        protected void Page_Load(object sender, EventArgs e) {
            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");
            Lbl_Ecabezado_Mensaje.Text = "";
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = false;
            if (!IsPostBack) {
                Llenar_Grid_Proveedores(0);
                Llenar_Combos();
                Llenar_Combos_MPE_Busquedas();
                Llenar_Combo_Procedencias();
                Grid_Listado_Bienes.Columns[1].Visible = false;
                Configuracion_Formulario(true);
                Div_Producto_Bien_Mueble_Padre.Visible = false;
                Div_Vehiculo_Parent.Visible = false;
            }
        }

    #endregion

    #region Metodos

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Llenar_Combos
        ///DESCRIPCIÓN: Se llenan los Combos Generales Independientes.
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 02/Diciembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Llenar_Combos() {
            try {
                Cls_Ope_Pat_Com_Bienes_Muebles_Negocio Combos = new Cls_Ope_Pat_Com_Bienes_Muebles_Negocio();

                //SE LLENA EL COMBO DE MATERIALES
                Combos.P_Tipo_DataTable = "MATERIALES";
                DataTable Materiales = Combos.Consultar_DataTable();
                Cmb_Materiales.DataSource = Materiales.Copy();
                Cmb_Materiales.DataValueField = "MATERIAL_ID";
                Cmb_Materiales.DataTextField = "DESCRIPCION";
                Cmb_Materiales.DataBind();
                Cmb_Materiales.Items.Insert(0, new ListItem("<SELECCIONE>", ""));

                Cmb_Material_Parte.DataSource = Materiales.Copy();
                Cmb_Material_Parte.DataValueField = "MATERIAL_ID";
                Cmb_Material_Parte.DataTextField = "DESCRIPCION";
                Cmb_Material_Parte.DataBind();
                Cmb_Material_Parte.Items.Insert(0, new ListItem("<SELECCIONE>", ""));

                Cmb_Material_Parent.DataSource = Materiales.Copy();
                Cmb_Material_Parent.DataValueField = "MATERIAL_ID";
                Cmb_Material_Parent.DataTextField = "DESCRIPCION";
                Cmb_Material_Parent.DataBind();
                Cmb_Material_Parent.Items.Insert(0, new ListItem("<SELECCIONE>", ""));

                Combos.P_Tipo_DataTable = "COLORES";
                DataTable Colores = Combos.Consultar_DataTable();
                DataRow Fila_Color = Colores.NewRow();
                Fila_Color["COLOR_ID"] = "SELECCIONE";
                Fila_Color["DESCRIPCION"] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
                Colores.Rows.InsertAt(Fila_Color, 0);
                Cmb_Colores.DataSource = Colores.Copy();
                Cmb_Colores.DataValueField = "COLOR_ID";
                Cmb_Colores.DataTextField = "DESCRIPCION";
                Cmb_Colores.DataBind();
                Cmb_Color_Parte.DataSource = Colores.Copy();
                Cmb_Color_Parte.DataValueField = "COLOR_ID";
                Cmb_Color_Parte.DataTextField = "DESCRIPCION";
                Cmb_Color_Parte.DataBind();
                Cmb_Color_Parent.DataSource = Colores.Copy();
                Cmb_Color_Parent.DataValueField = "COLOR_ID";
                Cmb_Color_Parent.DataTextField = "DESCRIPCION";
                Cmb_Color_Parent.DataBind();

                //SE LLENA EL COMBO DE MARCAS
                Combos.P_Tipo_DataTable = "MARCAS";
                DataTable Marcas = Combos.Consultar_DataTable();
                Cmb_Marca.DataSource = Marcas.Copy();
                Cmb_Marca.DataValueField = "MARCA_ID";
                Cmb_Marca.DataTextField = "NOMBRE";
                Cmb_Marca.DataBind();
                Cmb_Marca.Items.Insert(0, new ListItem("<SELECCIONE>", ""));
                Cmb_Marca_Parent.DataSource = Marcas.Copy();
                Cmb_Marca_Parent.DataValueField = "MARCA_ID";
                Cmb_Marca_Parent.DataTextField = "NOMBRE";
                Cmb_Marca_Parent.DataBind();
                Cmb_Marca_Parent.Items.Insert(0, new ListItem("<SELECCIONE>", ""));

                //SE LLENA EL COMBO DE DEPENDENCIAS
                Combo.P_Tipo_DataTable = "DEPENDENCIAS";
                DataTable Dependencias = Combo.Consultar_DataTable();
                Cmb_Dependencias.DataSource = Dependencias.Copy();
                Cmb_Dependencias.DataValueField = "DEPENDENCIA_ID";
                Cmb_Dependencias.DataTextField = "NOMBRE";
                Cmb_Dependencias.DataBind();
                Cmb_Dependencias.Items.Insert(0, new ListItem("<SELECCIONE>", ""));

                Combo.P_Tipo_DataTable = "ZONAS";
                DataTable Zonas = Combo.Consultar_DataTable();
                Zonas.DefaultView.Sort = "DESCRIPCION";
                Cmb_Zonas.DataSource = Zonas.Copy();
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
        ///FECHA_CREO: 02/Diciembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Llenar_Combo_Empleados() {
            try {
                Cmb_Empleados.SelectedIndex = (-1);
                DataTable Tabla = new DataTable();
                if (Cmb_Dependencias.SelectedIndex > 0) {
                    Cls_Ope_Pat_Com_Bienes_Muebles_Negocio Empleados = new Cls_Ope_Pat_Com_Bienes_Muebles_Negocio();
                    Empleados.P_Tipo_DataTable = "EMPLEADOS";
                    Empleados.P_Dependencia_ID = Cmb_Dependencias.SelectedItem.Value;
                    Tabla = Empleados.Consultar_DataTable();            
                }
                Cmb_Empleados.DataSource = Tabla;
                Cmb_Empleados.DataValueField = "EMPLEADO_ID";
                Cmb_Empleados.DataTextField = "NOMBRE";
                Cmb_Empleados.DataBind();
                Cmb_Empleados.Items.Insert(0, new ListItem("<- SELECCIONE ->", ""));
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
        ///FECHA_CREO: 30/Noviembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Llenar_Combo_Empleados(DataTable Tabla)
        {
            try
            {
                Cmb_Empleados.DataSource = Tabla;
                Cmb_Empleados.DataValueField = "EMPLEADO_ID";
                Cmb_Empleados.DataTextField = "NOMBRE";
                Cmb_Empleados.DataBind();
                Cmb_Empleados.Items.Insert(0, new ListItem("<- SELECCIONE ->", ""));
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
        private void Llenar_Combo_Empleados_Busqueda(DataTable Tabla) {
            try {
                DataRow Fila_Empleado = Tabla.NewRow();
                Fila_Empleado["EMPLEADO_ID"] = "SELECCIONE";
                Fila_Empleado["NOMBRE"] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
                Tabla.Rows.InsertAt(Fila_Empleado, 0);
                Cmb_Busqueda_Nombre_Resguardante.DataSource = Tabla;
                Cmb_Busqueda_Nombre_Resguardante.DataValueField = "EMPLEADO_ID";
                Cmb_Busqueda_Nombre_Resguardante.DataTextField = "NOMBRE";
                Cmb_Busqueda_Nombre_Resguardante.DataBind();
            } catch (Exception Ex) {
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
            Cmb_Procedencia.Items.Insert(0, new ListItem("<-- SELECCIONE -->", ""));
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Llenar_Combos
        ///DESCRIPCIÓN: Se llenan los Combos Generales Independientes.
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 01/Diciembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Llenar_Combos_MPE_Busquedas() {
            try {
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
                
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
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
        private void Llenar_Grid_Listado_Bienes(Int32 Pagina) {
            try{
                Grid_Listado_Bienes.Columns[1].Visible = true;
                Cls_Ope_Pat_Com_Bienes_Muebles_Negocio Bienes = new Cls_Ope_Pat_Com_Bienes_Muebles_Negocio();
                Bienes.P_Tipo_DataTable = "BIENES";
                if (Session["FILTRO_BUSQUEDA"] != null) {
                    Bienes.P_Tipo_Filtro_Busqueda = Session["FILTRO_BUSQUEDA"].ToString();
                    if (Session["FILTRO_BUSQUEDA"].ToString().Trim().Equals("DATOS_GENERALES")) {
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
        ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Resultados_Multiples
        ///DESCRIPCIÓN: Se llenan el Grid de Resultados Multiples del Modal de Busqueda 
        ///             dependiendo de los filtros pasados.
        ///PROPIEDADES:  Datos.  Fuente de donde se llenara el Grid.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 13/Septiembre/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Llenar_Grid_Resultados_Multiples(DataTable Datos) {
            try{
                Grid_Resultados_Multiples.Columns[1].Visible = true;
                Grid_Resultados_Multiples.DataSource = Datos;
                Grid_Resultados_Multiples.DataBind();
                Grid_Resultados_Multiples.Columns[1].Visible = false;
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }
        
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Limpiar_Generales
        ///DESCRIPCIÓN: Se Limpian los campos Generales de los Bienes Muebles.
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 02/Diciembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Limpiar_Generales() {
            try {
                Hdf_Bien_Mueble_ID.Value = "";
                Cmb_Clase_Activo.SelectedIndex = 0;
                Cmb_Tipo_Activo.SelectedIndex = 0;
                Txt_Fecha_Adquisicion.Text = "";
                Txt_Nombre_Producto.Text = "";
                Hdf_Proveedor_ID.Value = "";
                Txt_Nombre_Proveedor.Text = "";
                Cmb_Procedencia.SelectedIndex = 0;
                Cmb_Dependencias.SelectedIndex = 0;
                Cmb_Estatus.SelectedIndex = 0;
                Cmb_Zonas.SelectedIndex = 0;
                Txt_Garantia.Text = "";
                Txt_Modelo.Text = "";
                Cmb_Materiales.SelectedIndex = 0;
                Cmb_Colores.SelectedIndex = 0;
                Txt_Factura.Text = "";
                Txt_Numero_Serie.Text = "";
                Txt_Costo_Inicial.Text = "";
                Txt_Costo_Actual.Text = "";
                Cmb_Estatus.SelectedIndex = 0;
                Cmb_Estado.SelectedIndex = 0;
                Txt_Motivo_Baja.Text = "";
                Txt_Observaciones.Text = "";
                Txt_Numero_Inventario.Text = "";
                Txt_Invenario_Anterior.Text = "";
                Grid_Resguardantes.DataSource = new DataTable();
                Grid_Resguardantes.DataBind();
                Grid_Resguardantes.SelectedIndex = (-1);
                Grid_Historial_Resguardantes.DataSource = new DataTable();
                Grid_Historial_Resguardantes.DataBind();
                Grid_Historial_Resguardantes.SelectedIndex = (-1);
                Grid_Partes.DataSource = new DataTable();
                Grid_Partes.DataBind();
                Grid_Partes.SelectedIndex = (-1);
                Limpiar_Resguardantes();
                Limpiar_Historial_Resguardantes();
                Llenar_Combo_Empleados();
                Limpiar_SubBienes();
                Remover_Sesiones_Control_AsyncFileUpload(AFU_Archivo.ClientID);
                Limpiar_Parent();
                Div_Producto_Bien_Mueble_Padre.Visible = false;
                Div_Vehiculo_Parent.Visible = false;
                Txt_Busqueda_No_Empleado.Text = "";
                Txt_Busqueda_RFC.Text = "";
                Txt_Busqueda_Nombre_Empleado.Text = "";
                Cmb_Busqueda_Dependencia.SelectedIndex = 0;
                Grid_Busqueda_Empleados_Resguardo.DataSource = new DataTable();
                Grid_Busqueda_Empleados_Resguardo.DataBind();
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Limpiar_Resguardantes
        ///DESCRIPCIÓN: Se Limpian los campos de Resguardantes de los Bienes Muebles.
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 02/Diciembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Limpiar_Resguardantes() {
            try {
                Cmb_Empleados.SelectedIndex = 0;
                Txt_Cometarios.Text = "";
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Limpiar_Historial_Resguardantes
        ///DESCRIPCIÓN: Se Limpian los campos de Historial de los Resguardantes de los 
        ///             Bienes Muebles.
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 13/Diciembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Limpiar_Historial_Resguardantes() {
            try {
                Txt_Historial_Empleado_Resguardo.Text = "";
                Txt_Historial_Fecha_Inicial_Resguardo.Text = "";
                Txt_Historial_Fecha_Final_Resguardo.Text = "";
                Txt_Historial_Comentarios_Resguardo.Text = "";
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }
    
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Limpiar_SubBienes
        ///DESCRIPCIÓN: Limpia los campos donde se muestran los detalles de los subbienmes del bien.
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 24/Marzo/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Limpiar_SubBienes() {
            try {
                Txt_Numero_Inventario_Parte.Text = "";
                Cmb_Material_Parte.SelectedIndex = 0;
                Cmb_Color_Parte.SelectedIndex = 0;
                Txt_Nombre_Parte.Text = "";
                Txt_Costo_Parte.Text = "";
                Txt_Fecha_Adquisicion_Parte.Text = "";
                Cmb_Estado_Parte.SelectedIndex = 0; 
                Cmb_Estatus_Parte.SelectedIndex = 0;
                Txt_Comentarios_Parte.Text = "";
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
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
            try {
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
                Txt_Vehiculo_Nombre.Text = "";
                Txt_Vehiculo_No_Inventario.Text = "";
                Txt_Vehiculo_Numero_Serie.Text = "";
                Txt_Vehiculo_Marca.Text = "";
                Txt_Vehiculo_Tipo.Text = "";
                Txt_Vehiculo_Color.Text = "";
                Txt_Vehiculo_Modelo.Text = "";
                Txt_Vehiculo_Numero_Economico.Text = "";
                Txt_Vehiculo_Placas.Text = "";
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
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
        private Boolean Buscar_Clave_DataTable(String Clave, DataTable Tabla, Int32 Columna) {
            Boolean Resultado_Busqueda = false;
            if (Tabla != null && Tabla.Rows.Count > 0 && Tabla.Columns.Count > 0) {
                if (Tabla.Columns.Count > Columna) {
                    for (Int32 Contador = 0; Contador < Tabla.Rows.Count; Contador++) {
                        if (Tabla.Rows[Contador][Columna].ToString().Trim().Equals(Clave.Trim())) {
                            Resultado_Busqueda = true;
                            break;
                        }
                    }
                }
            }
            return Resultado_Busqueda;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Resguardantes
        ///DESCRIPCIÓN: Llena la tabla de Resguardantes
        ///PROPIEDADES:     
        ///             1.  Pagina. Pagina en la cual se mostrará el Grid_VIew
        ///             2.  Tabla.  Tabla que se va a cargar en el Grid.    
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 02/Diciembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Llenar_Grid_Resguardantes(Int32 Pagina, DataTable Tabla) {
            try {
                Session["Dt_Resguardantes"] = Tabla;
                Grid_Resguardantes.Columns[1].Visible = true;
                Grid_Resguardantes.Columns[2].Visible = true;
                Grid_Resguardantes.DataSource = Tabla;
                Grid_Resguardantes.PageIndex = Pagina;
                Grid_Resguardantes.DataBind();
                Grid_Resguardantes.Columns[1].Visible = false;
                Grid_Resguardantes.Columns[2].Visible = false;
            } catch (Exception Ex) {    
                Lbl_Ecabezado_Mensaje.Text = "Aqui";
                Lbl_Mensaje_Error.Text = Ex.Message;
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Historial_Resguardos
        ///DESCRIPCIÓN: Llena la tabla de Historial de Resguardantes
        ///PROPIEDADES:     
        ///             1.  Pagina. Pagina en la cual se mostrará el Grid_VIew
        ///             2.  Tabla.  Tabla que se va a cargar en el Grid.    
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 13/Diciembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Llenar_Grid_Historial_Resguardos(Int32 Pagina, DataTable Tabla) {
            Grid_Historial_Resguardantes.Columns[1].Visible = true;
            Grid_Historial_Resguardantes.Columns[2].Visible = true;
            Grid_Historial_Resguardantes.DataSource = Tabla;
            Grid_Historial_Resguardantes.PageIndex = Pagina;
            Grid_Historial_Resguardantes.DataBind();
            Grid_Historial_Resguardantes.Columns[1].Visible = false;
            Grid_Historial_Resguardantes.Columns[2].Visible = false;
            Session["Dt_Historial_Resguardos"] = Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Historial_Archivos
        ///DESCRIPCIÓN: Llena la tabla de Historial de Archivos
        ///PROPIEDADES:     
        ///             1.  Pagina. Pagina en la cual se mostrará el Grid_VIew
        ///             2.  Tabla.  Tabla que se va a cargar en el Grid.    
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 16/Febrero/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Llenar_Grid_Historial_Archivos(Int32 Pagina, DataTable Tabla)
        {
            Grid_Archivos.Columns[0].Visible = true;
            Grid_Archivos.Columns[1].Visible = true;
            Grid_Archivos.DataSource = Tabla;
            Grid_Archivos.PageIndex = Pagina;
            Grid_Archivos.DataBind();
            Grid_Archivos.Columns[0].Visible = false;
            Grid_Archivos.Columns[1].Visible = false;
            Session["Dt_Historial_Archivos"] = Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Partes_Bienes
        ///DESCRIPCIÓN: Llena la tabla de Partes de Bienes
        ///PROPIEDADES:     
        ///             1.  Pagina. Pagina en la cual se mostrará el Grid_VIew
        ///             2.  Tabla.  Tabla que se va a cargar en el Grid.    
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 23/Marzo/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Llenar_Grid_Partes_Bienes(Int32 Pagina, DataTable Tabla) {
            Grid_Partes.Columns[1].Visible = true;
            Grid_Partes.DataSource = Tabla;
            Grid_Partes.PageIndex = Pagina;
            Grid_Partes.DataBind();
            Grid_Partes.Columns[1].Visible = false;
            Session["Dt_Sub_Bienes"] = Tabla;
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
        private void Llenar_Grid_Proveedores(Int32 Pagina) {
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
        ///NOMBRE DE LA FUNCIÓN: Configuracion_Formulario
        ///DESCRIPCIÓN: Carga una configuracion de los controles del Formulario
        ///PROPIEDADES:     
        ///             1. Estatus. Estatus en el que se cargara la configuración de los 
        ///                         controles.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 02/Diciembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Configuracion_Formulario(Boolean Estatus) {
            if (Estatus) {
                Btn_Modificar.AlternateText = "Modificar";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                Btn_Salir.AlternateText = "Salir";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                Btn_Generar_Reporte.Visible = true;
            } else {
                Btn_Modificar.AlternateText = "Actualizar";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                Btn_Salir.AlternateText = "Cancelar";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                Btn_Generar_Reporte.Visible = false;
            }
            Cmb_Dependencias.Enabled = !Estatus;
            Cmb_Tipo_Activo.Enabled = !Estatus;
            Cmb_Clase_Activo.Enabled = !Estatus;
            Btn_Lanzar_Mpe_Proveedores.Visible = !Estatus;
            Cmb_Marca.Enabled = !Estatus;
            Btn_Fecha_Adquisicion.Enabled = !Estatus;
            Txt_Modelo.Enabled = !Estatus;
            Cmb_Procedencia.Enabled = !Estatus;
            Txt_Garantia.Enabled = !Estatus;
            Cmb_Materiales.Enabled = !Estatus;
            Cmb_Zonas.Enabled = !Estatus;
            Cmb_Colores.Enabled = !Estatus;
            Txt_Factura.Enabled = !Estatus;
            Txt_Numero_Serie.Enabled = !Estatus;
            Cmb_Estatus.Enabled = !Estatus;
            Cmb_Estado.Enabled = !Estatus;
            Txt_Motivo_Baja.Enabled = !Estatus;
            Txt_Observaciones.Enabled = !Estatus;
            if (!Div_Producto_Bien_Mueble_Padre.Visible && !Div_Vehiculo_Parent.Visible) {
                Cmb_Empleados.Enabled = !Estatus;
                Txt_Cometarios.Enabled = !Estatus;
                Btn_Agregar_Resguardante.Visible = !Estatus;
                Btn_Quitar_Resguardante.Visible = !Estatus;
                Grid_Resguardantes.Columns[0].Visible = !Estatus;
                Btn_Busqueda_Avanzada_Resguardante.Visible = !Estatus;
            } else {
                Cmb_Empleados.Enabled = false;
                Txt_Cometarios.Enabled = false;
                Btn_Agregar_Resguardante.Visible = false;
                Btn_Quitar_Resguardante.Visible = false;
                Grid_Resguardantes.Columns[0].Visible = false;
                Btn_Busqueda_Avanzada_Resguardante.Visible = false;
            }
            Div_Busqueda.Visible = Estatus;
            AFU_Archivo.Enabled = !Estatus;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Mostrar_Detalles_Bien_Mueble
        ///DESCRIPCIÓN: Carga una configuracion de los controles del Formulario
        ///PROPIEDADES:     
        ///             1. Bien_Mueble. Contiene los parametros que se desean mostrar.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 13/Diciembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Mostrar_Detalles_Bien_Mueble(Cls_Ope_Pat_Com_Bienes_Muebles_Negocio Bien_Mueble){
            try {
                Hdf_Bien_Mueble_ID.Value = Bien_Mueble.P_Bien_Mueble_ID;
                Cmb_Tipo_Activo.SelectedIndex = Cmb_Tipo_Activo.Items.IndexOf(Cmb_Tipo_Activo.Items.FindByValue(Bien_Mueble.P_Clasificacion_ID));
                Cmb_Clase_Activo.SelectedIndex = Cmb_Clase_Activo.Items.IndexOf(Cmb_Clase_Activo.Items.FindByValue(Bien_Mueble.P_Clase_Activo_ID));
                Txt_Fecha_Adquisicion.Text = String.Format("{0:dd/MMM/yyyy}", Bien_Mueble.P_Fecha_Adquisicion_);
                Txt_Nombre_Producto.Text = Bien_Mueble.P_Nombre_Producto;
                //Cmb_Dependencias.SelectedIndex = Cmb_Dependencias.Items.IndexOf(Cmb_Dependencias.Items.FindByValue(Bien_Mueble.P_Dependencia_ID));
                for (Int32 Contador = 0; Contador < Bien_Mueble.P_Resguardantes.Rows.Count; Contador++) {
                    Cmb_Dependencias.SelectedIndex = Cmb_Dependencias.Items.IndexOf(Cmb_Dependencias.Items.FindByValue(Bien_Mueble.P_Resguardantes.Rows[Contador]["DEPENDENCIA_ID"].ToString().Trim()));
                }
                Cmb_Dependencias_SelectedIndexChanged(Cmb_Dependencias, null);
                Cmb_Marca.SelectedIndex = Cmb_Marca.Items.IndexOf(Cmb_Marca.Items.FindByValue(Bien_Mueble.P_Marca_ID));
                Txt_Garantia.Text = Bien_Mueble.P_Garantia;
                Txt_Modelo.Text = Bien_Mueble.P_Modelo;
                Cmb_Materiales.SelectedIndex = Cmb_Materiales.Items.IndexOf(Cmb_Materiales.Items.FindByValue(Bien_Mueble.P_Material_ID));
                Cmb_Colores.SelectedIndex = Cmb_Colores.Items.IndexOf(Cmb_Colores.Items.FindByValue(Bien_Mueble.P_Color_ID));
                Txt_Factura.Text = Bien_Mueble.P_Factura;
                Txt_Numero_Serie.Text = Bien_Mueble.P_Numero_Serie;
                Txt_Costo_Inicial.Text = Bien_Mueble.P_Costo_Inicial.ToString("#######0.00");
                Txt_Costo_Actual.Text = Bien_Mueble.P_Costo_Actual.ToString("#######0.00");
                Cmb_Estatus.SelectedIndex = Cmb_Estatus.Items.IndexOf(Cmb_Estatus.Items.FindByValue(Bien_Mueble.P_Estatus));
                Cmb_Procedencia.SelectedIndex = Cmb_Procedencia.Items.IndexOf(Cmb_Procedencia.Items.FindByValue(Bien_Mueble.P_Procedencia));
                Txt_Motivo_Baja.Text = Bien_Mueble.P_Motivo_Baja;
                Cmb_Estado.SelectedIndex = Cmb_Estado.Items.IndexOf(Cmb_Estado.Items.FindByValue(Bien_Mueble.P_Estado));
                Cmb_Zonas.SelectedIndex = Cmb_Zonas.Items.IndexOf(Cmb_Zonas.Items.FindByValue(Bien_Mueble.P_Zona));
                Txt_Observaciones.Text = Bien_Mueble.P_Observaciones;
                Txt_Usuario_creo.Text = (Bien_Mueble.P_Dato_Creacion.Trim() != "[]") ? Bien_Mueble.P_Dato_Creacion : "";
                Txt_Usuario_Modifico.Text = (Bien_Mueble.P_Dato_Modificacion.Trim() != "[]") ? Bien_Mueble.P_Dato_Modificacion : "";
                Txt_Numero_Inventario.Text = Bien_Mueble.P_Numero_Inventario;
                Txt_Invenario_Anterior.Text = Bien_Mueble.P_Numero_Inventario_Anterior;
                Txt_Resguardo_Recibo.Text = Bien_Mueble.P_Operacion.Trim();
                if (Bien_Mueble.P_Operacion.Trim() != null && Bien_Mueble.P_Operacion.Trim().Length>0)
                    Session["OPERACION_BM"] = Bien_Mueble.P_Operacion.Trim();
                else
                    Session["OPERACION_BM"] = null;
                if (Bien_Mueble.P_Proveedor_ID != null && Bien_Mueble.P_Proveedor_ID.Trim().Length > 0) {
                    Cls_Cat_Com_Proveedores_Negocio Proveedor_Negocio = new Cls_Cat_Com_Proveedores_Negocio();
                    Proveedor_Negocio.P_Proveedor_ID = Bien_Mueble.P_Proveedor_ID;
                    DataTable Dt_Proveedor = Proveedor_Negocio.Consulta_Datos_Proveedores();
                    if (Dt_Proveedor != null && Dt_Proveedor.Rows.Count > 0) {
                        Hdf_Proveedor_ID.Value = Dt_Proveedor.Rows[0][Cat_Com_Proveedores.Campo_Proveedor_ID].ToString();
                        Txt_Nombre_Proveedor.Text = Dt_Proveedor.Rows[0][Cat_Com_Proveedores.Campo_Nombre].ToString();
                    }
                }
                Llenar_Combo_Empleados();
                Llenar_Grid_Resguardantes(0, Bien_Mueble.P_Resguardantes);
                Llenar_Grid_Historial_Resguardos(0, Bien_Mueble.P_Historial_Resguardos);
                Llenar_Grid_Historial_Archivos(0, Bien_Mueble.P_Dt_Historial_Archivos);
                Llenar_Grid_Partes_Bienes(0, Bien_Mueble.P_Dt_Bienes_Dependientes);
                if (Bien_Mueble.P_Ascencendia != null && Bien_Mueble.P_Ascencendia.Trim().Length > 0) {
                    if (Bien_Mueble.P_Proveniente != null && Bien_Mueble.P_Proveniente.Trim().Equals("BIEN_MUEBLE")) {
                        Cls_Ope_Pat_Com_Bienes_Muebles_Negocio Bien_Parent = new Cls_Ope_Pat_Com_Bienes_Muebles_Negocio();
                        Bien_Parent.P_Bien_Mueble_ID = Bien_Mueble.P_Ascencendia;
                        Bien_Parent = Bien_Parent.Consultar_Detalles_Bien_Mueble();
                        Mostrar_Detalles_Bien_Parent(Bien_Parent);
                    } else if (Bien_Mueble.P_Proveniente != null && Bien_Mueble.P_Proveniente.Trim().Equals("VEHICULO")) {
                        Cls_Ope_Pat_Com_Vehiculos_Negocio Vehiculo = new Cls_Ope_Pat_Com_Vehiculos_Negocio();
                        Vehiculo.P_Vehiculo_ID = Bien_Mueble.P_Ascencendia;
                        Vehiculo = Vehiculo.Consultar_Detalles_Vehiculo();
                        Mostrar_Detalles_Vehiculo(Vehiculo);
                    }
                } else {
                    Limpiar_Parent();
                    Div_Producto_Bien_Mueble_Padre.Visible = false;
                    Div_Vehiculo_Parent.Visible = false;
                }
                Tab_Contenedor_Pestagnas.ActiveTabIndex = 0;
                System.Threading.Thread.Sleep(1000);
            }
            catch (Exception Ex)
            {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Mostrar_Detalles_Bien_Parent
        ///DESCRIPCIÓN: Muestra los detalles del Bien Mueble Parent
        ///PROPIEDADES:     
        ///             1. Bien_Mueble. Contiene los parametros que se desean mostrar.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 25/Marzo/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Mostrar_Detalles_Bien_Parent(Cls_Ope_Pat_Com_Bienes_Muebles_Negocio Bien_Mueble){
            try {
                Limpiar_Parent();
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
                Txt_Observaciones_Parent.Text = Bien_Mueble.P_Observaciones;
                Div_Producto_Bien_Mueble_Padre.Visible = true;
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
        private void Mostrar_Detalles_Vehiculo(Cls_Ope_Pat_Com_Vehiculos_Negocio Vehiculo)
        {
            if (Vehiculo.P_Vehiculo_ID != null && Vehiculo.P_Vehiculo_ID.Trim().Length > 0)
            {
                Limpiar_Parent();
                Hdf_Bien_Padre_ID.Value = Vehiculo.P_Vehiculo_ID;
                Txt_Vehiculo_Nombre.Text = Vehiculo.P_Nombre_Producto;
                Cmb_Dependencias.SelectedIndex = Cmb_Dependencias.Items.IndexOf(Cmb_Dependencias.Items.FindByValue(Vehiculo.P_Dependencia_ID));
                Txt_Vehiculo_No_Inventario.Text = Vehiculo.P_Numero_Inventario.ToString();
                Txt_Vehiculo_Numero_Serie.Text = Vehiculo.P_Serie_Carroceria.Trim();
                if (Vehiculo.P_Marca_ID != null && Vehiculo.P_Marca_ID.Trim().Length > 0)
                {
                    Cls_Cat_Com_Marcas_Negocio Marca_Negocio = new Cls_Cat_Com_Marcas_Negocio();
                    Marca_Negocio.P_Marca_ID = Vehiculo.P_Marca_ID;
                    DataTable Dt_Marcas = Marca_Negocio.Consulta_Marcas();
                    if (Dt_Marcas != null && Dt_Marcas.Rows.Count > 0)
                    {
                        Txt_Vehiculo_Marca.Text = Dt_Marcas.Rows[0][Cat_Com_Marcas.Campo_Nombre].ToString();
                    }
                }
                Txt_Vehiculo_Modelo.Text = Vehiculo.P_Modelo_ID;
                if (Vehiculo.P_Tipo_Vehiculo_ID != null && Vehiculo.P_Tipo_Vehiculo_ID.Trim().Length > 0)
                {
                    Cls_Cat_Pat_Com_Tipos_Vehiculo_Negocio Tipo_Negocio = new Cls_Cat_Pat_Com_Tipos_Vehiculo_Negocio();
                    Tipo_Negocio.P_Tipo_Vehiculo_ID = Vehiculo.P_Tipo_Vehiculo_ID;
                    Tipo_Negocio = Tipo_Negocio.Consultar_Datos_Vehiculo();
                    if (Tipo_Negocio.P_Tipo_Vehiculo_ID != null && Tipo_Negocio.P_Tipo_Vehiculo_ID.Trim().Length > 0)
                    {
                        Txt_Vehiculo_Tipo.Text = Tipo_Negocio.P_Descripcion;
                    }
                }
                if (Vehiculo.P_Color_ID != null && Vehiculo.P_Color_ID.Trim().Length > 0)
                {
                    Cls_Cat_Pat_Com_Colores_Negocio Color_Negocio = new Cls_Cat_Pat_Com_Colores_Negocio();
                    Color_Negocio.P_Color_ID = Vehiculo.P_Color_ID;
                    Color_Negocio.P_Tipo_DataTable = "COLORES";
                    DataTable Dt_Colores = Color_Negocio.Consultar_DataTable();
                    if (Dt_Colores != null && Dt_Colores.Rows.Count > 0)
                    {
                        Txt_Vehiculo_Color.Text = Dt_Colores.Rows[0][Cat_Pat_Colores.Campo_Descripcion].ToString();
                    }
                }
                Txt_Vehiculo_Numero_Economico.Text = Vehiculo.P_Numero_Economico_;
                Txt_Vehiculo_Placas.Text = Vehiculo.P_Placas;
                Div_Vehiculo_Parent.Visible = true;
            }
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
        private void Remover_Sesiones_Control_AsyncFileUpload(String Cliente_ID) {
            HttpContext Contexto;
            if (HttpContext.Current != null && HttpContext.Current.Session != null) {
                Contexto = HttpContext.Current;
            }  else {
                Contexto = null;
            }
            if (Contexto != null) {
                foreach (String key in Contexto.Session.Keys) {
                    if (key.Contains(Cliente_ID)) {
                        Contexto.Session.Remove(key);
                        break;
                    }
                }
            }
        }

        #region Validaciones

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Validar_Componentes_Generales
            ///DESCRIPCIÓN: Hace una validacion de que haya datos en los componentes antes de hacer
            ///             una operación de Actualizacion.
            ///PROPIEDADES:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 02/Diciembre/2010 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            private bool Validar_Componentes_Generales() {
                Lbl_Ecabezado_Mensaje.Text = "Es necesario.";
                String Mensaje_Error = "";
                Boolean Validacion = true;
                //if (Cmb_Clase_Activo.SelectedIndex == 0)
                //{
                //    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                //    Mensaje_Error = Mensaje_Error + "+ Debe seleccionar una opción del Combo de Clase de Activo.";
                //    Validacion = false;
                //}
                //if (Cmb_Tipo_Activo.SelectedIndex == 0)
                //{
                //    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                //    Mensaje_Error = Mensaje_Error + "+ Debe seleccionar una opción del Combo de Tipo de Activo.";
                //    Validacion = false;
                //}
                if (Cmb_Materiales.SelectedIndex == 0) {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                    Mensaje_Error = Mensaje_Error + "+ Debe seleccionar una opción del Combo de Materiales.";
                    Validacion = false;
                }
                if (Cmb_Procedencia.SelectedIndex == 0)
                {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                    Mensaje_Error = Mensaje_Error + "+ Debe seleccionar una opción del Combo de Procedencias.";
                    Validacion = false;
                }
                if (Cmb_Colores.SelectedIndex == 0) {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                    Mensaje_Error = Mensaje_Error + "+ Debe seleccionar una opción del Combo de Colores.";
                    Validacion = false;
                }
                if (Txt_Factura.Text.Trim().Length == 0) {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                    Mensaje_Error = Mensaje_Error + "+ Introducir la Factura del Bien.";
                    Validacion = false;
                }
                if (Txt_Numero_Serie.Text.Trim().Length == 0) {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                    Mensaje_Error = Mensaje_Error + "+ Introducir el Número de Serie del Bien.";
                    Validacion = false;
                }
                if (Cmb_Estatus.SelectedIndex == 0) {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                    Mensaje_Error = Mensaje_Error + "+ Debe seleccionar una opción del Combo de Estatus.";
                    Validacion = false;
                } else {
                    if (!Cmb_Estatus.SelectedItem.Value.Equals("VIGENTE")) {
                        if (Txt_Motivo_Baja.Text.Trim().Length == 0) {
                            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                            Mensaje_Error = Mensaje_Error + "+ Introducir la Motivo de Baja del Bien.";
                            Validacion = false;                                    
                        }
                    }
                }
                if (Cmb_Estado.SelectedIndex == 0) {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                    Mensaje_Error = Mensaje_Error + "+ Debe seleccionar una opción del Combo de Estado.";
                    Validacion = false;
                }
                if (Grid_Resguardantes.Rows.Count == 0 || Session["Dt_Resguardantes"] == null) {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                    Mensaje_Error = Mensaje_Error + "+ Debe haber como minimo un empleado para resguardo del Bien.";
                    Validacion = false;
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
            ///FECHA_CREO: 02/Diciembre/2010 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            private bool Validar_Componentes_Resguardos() {
                Lbl_Ecabezado_Mensaje.Text = "Es necesario.";
                String Mensaje_Error = "";
                Boolean Validacion = true;
                if (Cmb_Empleados.SelectedIndex == 0) {
                    Mensaje_Error = Mensaje_Error + "+ Seleccionar el Empleado para Resguardo.";
                    Validacion = false;
                }
                if (!Validacion) {
                    Lbl_Mensaje_Error.Text = HttpUtility.HtmlDecode(Mensaje_Error);
                    Div_Contenedor_Msj_Error.Visible = true;
                }
                return Validacion;
            }


            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Validar_Baja_Bien
            ///DESCRIPCIÓN: Valida las Bajas de los bienes
            ///PROPIEDADES:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: marzo/2011 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            private bool Validar_Baja_Bien()
            {
                Lbl_Ecabezado_Mensaje.Text = "Es necesario.";
                String Mensaje_Error = "";
                Boolean Validacion = true;
                if (Cmb_Estatus.SelectedItem.Value != "VIGENTE")
                {
                    Cls_Ope_Pat_Com_Bienes_Muebles_Negocio Negocio = new Cls_Ope_Pat_Com_Bienes_Muebles_Negocio();
                    Negocio.P_Bien_Mueble_ID = Hdf_Bien_Mueble_ID.Value;
                    Negocio = Negocio.Consultar_Detalles_Bien_Mueble();
                    if (Negocio.P_Resguardantes == null || Negocio.P_Resguardantes.Rows.Count == 0)
                    {
                        if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                        Mensaje_Error = Mensaje_Error + "+ Antes de dar una Baja es necesario Actualizarle un Resguardate al Bien Mueble.";
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

        #endregion

        #region Reporte

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
                Consulta_Resguardos_Negocio = new Cls_Alm_Com_Resguardos_Negocio();
                Bien_Id.P_Producto_Almacen = false;
                DataTable Data_Set_Resguardos_Bienes;
                Consulta_Resguardos_Negocio.P_Operacion = Bien_Id.P_Operacion.Trim();

                if (Session["OPERACION_BM"] != null)
                    Consulta_Resguardos_Negocio.P_Operacion = Session["OPERACION_BM"].ToString().Trim();

                Data_Set_Resguardos_Bienes = Consulta_Resguardos_Negocio.Consulta_Resguardos_Bienes(Bien_Id);

                Ds_Alm_Com_Resguardos_Bienes Ds_Consulta_Resguardos_Bienes = new Ds_Alm_Com_Resguardos_Bienes();
                Generar_Reporte(Data_Set_Resguardos_Bienes, Ds_Consulta_Resguardos_Bienes, Formato);
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
            private void Generar_Reporte(DataTable Data_Set_Consulta_DB, DataSet Ds_Reporte, String Formato)
            {

                String Ruta_Reporte_Crystal = "";
                String Nombre_Reporte_Generar = "";
                DataRow Renglon;

                try
                {
                    Renglon = Data_Set_Consulta_DB.Rows[0];
                    String Cantidad = Data_Set_Consulta_DB.Rows[0]["CANTIDAD"].ToString();
                    String Costo = Data_Set_Consulta_DB.Rows[0]["COSTO_UNITARIO"].ToString();
                    Double Resultado = (Convert.ToDouble(Cantidad)) * (Convert.ToDouble(Costo));
                    Ds_Reporte.Tables[1].ImportRow(Renglon);
                    Ds_Reporte.Tables[1].Rows[0].SetField("COSTO_TOTAL", Resultado);

                    if (Session["OPERACION_BM"] != null)  
                    {
                        if (Session["OPERACION_BM"].ToString().Trim() == "RESGUARDO")
                            Ds_Reporte.Tables[1].Rows[0].SetField("OPERACION", "CONTROL DE RESGUARDOS DE BIENES MUEBLES");

                        else if (Session["OPERACION_BM"].ToString().Trim() == "RECIBO")
                            Ds_Reporte.Tables[1].Rows[0].SetField("OPERACION", "CONTROL DE RECIBOS DE BIENES MUEBLES");
                    }
                    if ((string.IsNullOrEmpty(Ds_Reporte.Tables[1].Rows[0]["MARCA"].ToString())) | (Ds_Reporte.Tables[1].Rows[0]["MARCA"].ToString().Trim() == ""))
                        Ds_Reporte.Tables[1].Rows[0].SetField("MARCA", "INDISTINTA");

                    if (string.IsNullOrEmpty(Ds_Reporte.Tables[1].Rows[0]["MODELO"].ToString()))
                        Ds_Reporte.Tables[1].Rows[0].SetField("MODELO", "INDISTINTO");



                    for (int Cont_Elementos = 0; Cont_Elementos < Data_Set_Consulta_DB.Rows.Count; Cont_Elementos++)
                    {
                        Renglon = Data_Set_Consulta_DB.Rows[Cont_Elementos];
                        
                        String Nombre_E = Data_Set_Consulta_DB.Rows[Cont_Elementos]["NOMBRE_E"].ToString();
                        String Apellido_Paterno_E = Data_Set_Consulta_DB.Rows[Cont_Elementos]["APELLIDO_PATERNO_E"].ToString();
                        String Apellido_Materno_E = Data_Set_Consulta_DB.Rows[Cont_Elementos]["APELLIDO_MATERNO_E"].ToString();
                        String RFC_E = Data_Set_Consulta_DB.Rows[Cont_Elementos]["RFC_E"].ToString();
                        String Resguardante = Nombre_E + " " + Apellido_Paterno_E + " " + Apellido_Materno_E + " " + "(" + RFC_E + ")";
                        if (!Resguardante.Trim().Equals("()")) {
                            Ds_Reporte.Tables[0].ImportRow(Renglon);
                            Ds_Reporte.Tables[0].Rows[Cont_Elementos].SetField("RESGUARDANTES", Resguardante);
                        }
                    }


                    // Ruta donde se encuentra el reporte Crystal
                    Ruta_Reporte_Crystal = "../Rpt/Almacen/Rpt_Alm_Com_Resguardos_Bienes_Almacen.rpt";

                    // Se crea el nombre del reporte
                    //String Nombre_Reporte;

                    // Se da el nombre del reporte que se va generar
                    if (Formato == "PDF")
                        Nombre_Reporte_Generar = "Rpt_Alm_Com_Resguardos_Bienes_Almacen" + Session.SessionID + ".pdf";  // Es el nombre del reporte PDF que se va a generar
                    else if (Formato == "Excel")
                        Nombre_Reporte_Generar = "Rpt_Alm_Com_Resguardos_Bienes_Almacen" + Session.SessionID + ".xls";  // Es el nombre del repote en Excel que se va a generar

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


        #endregion

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
            public void Consultar_Empleados(String Dependencia_ID)  {
                try  {
                    Session.Remove("Dt_Resguardantes");
                    Grid_Resguardantes.DataSource = new DataTable();
                    Grid_Resguardantes.DataBind();

                    Cls_Ope_Pat_Com_Bienes_Muebles_Negocio Combo = new Cls_Ope_Pat_Com_Bienes_Muebles_Negocio();
                    Combo.P_Tipo_DataTable = "EMPLEADOS";
                    Combo.P_Dependencia_ID = Dependencia_ID;
                    DataTable Tabla = Combo.Consultar_DataTable();
                    Llenar_Combo_Empleados(Tabla);
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
        
    #endregion

    #region Grid

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
            try{
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
            try{
                if (Grid_Listado_Bienes.SelectedIndex > (-1)) {
                    Limpiar_Generales();
                    String Bien_Mueble_Seleccionado_ID = Grid_Listado_Bienes.SelectedRow.Cells[1].Text.Trim();
                    Cls_Ope_Pat_Com_Bienes_Muebles_Negocio Bien_Mueble = new Cls_Ope_Pat_Com_Bienes_Muebles_Negocio();
                    Bien_Mueble.P_Bien_Mueble_ID = Bien_Mueble_Seleccionado_ID;
                    Bien_Mueble = Bien_Mueble.Consultar_Detalles_Bien_Mueble();
                    Mostrar_Detalles_Bien_Mueble(Bien_Mueble);
                    Grid_Listado_Bienes.SelectedIndex = -1;
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
        ///NOMBRE DE LA FUNCIÓN: Grid_Historial_Resguardantes_PageIndexChanging
        ///DESCRIPCIÓN: Maneja la paginación del GridView de Historial de Resguardos
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 13/Diciembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Historial_Resguardantes_PageIndexChanging(object sender, GridViewPageEventArgs e) {
            try {
                if (Session["Dt_Historial_Resguardos"] != null) { 
                    Grid_Historial_Resguardantes.SelectedIndex = (-1);
                    Llenar_Grid_Historial_Resguardos(e.NewPageIndex, (DataTable)Session["Dt_Historial_Resguardos"]);
                }
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Historial_Resguardantes_SelectedIndexChanged
        ///DESCRIPCIÓN: Maneja el evento de cambio de Seleccion del GridView de Historial
        ///             de Resguardos.
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 13/Diciembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Historial_Resguardantes_SelectedIndexChanged(object sender, EventArgs e) {
            try {
                if (Grid_Historial_Resguardantes.SelectedIndex > (-1)) {
                    Limpiar_Historial_Resguardantes();
                    if (Session["Dt_Historial_Resguardos"] != null) {
                        Int32 Registro = ((Grid_Historial_Resguardantes.PageIndex) * Grid_Historial_Resguardantes.PageSize) + (Grid_Historial_Resguardantes.SelectedIndex);
                        DataTable Tabla = (DataTable)Session["Dt_Historial_Resguardos"];
                        Txt_Historial_Empleado_Resguardo.Text = "[" + Tabla.Rows[Registro]["NO_EMPLEADO"].ToString().Trim() + "] " + Tabla.Rows[Registro]["NOMBRE_EMPLEADO"].ToString().Trim();
                        Txt_Historial_Comentarios_Resguardo.Text = Tabla.Rows[Registro]["COMENTARIOS"].ToString().Trim();
                        Txt_Historial_Fecha_Inicial_Resguardo.Text = String.Format("{0:dd/MMM/yyyy}", Tabla.Rows[Registro]["FECHA_INICIAL"]);
                        Txt_Historial_Fecha_Final_Resguardo.Text = String.Format("{0:dd/MMM/yyyy}", Tabla.Rows[Registro]["FECHA_FINAL"]);
                    }
                }
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Resguardantes_PageIndexChanging
        ///DESCRIPCIÓN: Maneja la paginación del GridView de Resguardos
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 13/Diciembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Resguardantes_PageIndexChanging(object sender, GridViewPageEventArgs e) {
            try {
                if (Session["Dt_Resguardantes"] != null) {
                    Grid_Resguardantes.SelectedIndex = (-1);
                    Llenar_Grid_Resguardantes(e.NewPageIndex, (DataTable)Session["Dt_Resguardantes"]);
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
        protected void Grid_Resguardantes_RowDataBound(object sender, GridViewRowEventArgs e) {
            try {
                if (e.Row.RowType == DataControlRowType.DataRow) { 
                    if(e.Row.FindControl("Btn_Ver_Informacion_Resguardo") != null){
                        if(Session["Dt_Resguardantes"] != null){
                            ImageButton Btn_Informacion = (ImageButton) e.Row.FindControl("Btn_Ver_Informacion_Resguardo");
                            Btn_Informacion.CommandArgument = ((DataTable)Session["Dt_Resguardantes"]).Rows[e.Row.RowIndex]["COMENTARIOS"].ToString();
                        }
                    }
                }
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = "Verificar.";
                Lbl_Ecabezado_Mensaje.Text = "[Excepción: '" + Ex.Message + "']";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Archivos_PageIndexChanging
        ///DESCRIPCIÓN: Maneja la paginación del GridView de Historial de Archivos
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 16/Febrero/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Archivos_PageIndexChanging(object sender, GridViewPageEventArgs e) {
            try {
                if (Session["Dt_Historial_Archivos"] != null) {
                    Grid_Archivos.SelectedIndex = (-1);
                    Llenar_Grid_Historial_Archivos(e.NewPageIndex, (DataTable)Session["Dt_Historial_Archivos"]);
                }
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Archivos_RowDataBound
        ///DESCRIPCIÓN: Maneja el evento de RowDataBound del Grid de Archivos
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 16/Febrero/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Archivos_RowDataBound(object sender, GridViewRowEventArgs e) {
            if (e.Row.RowType == DataControlRowType.DataRow) {
                ImageButton Boton = (ImageButton)e.Row.FindControl("Btn_Ver_Archivo");
                Boton.CommandArgument = e.Row.Cells[0].Text.Trim();
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Partes_PageIndexChanging
        ///DESCRIPCIÓN: Maneja el Cambio de Pagina de Grid_Partes
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 16/Marzo/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Partes_PageIndexChanging(object sender, GridViewPageEventArgs e) {
            try {
                DataTable Tabla = new DataTable();
                if (Session["Dt_Sub_Bienes"] != null) {
                    Tabla = (DataTable)Session["Dt_Sub_Bienes"];
                }
                Llenar_Grid_Partes_Bienes(e.NewPageIndex, Tabla);
            } catch (Exception Ex) {
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
        ///FECHA_CREO: 16/Marzo/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Partes_SelectedIndexChanged(object sender, EventArgs e) {
            try {
                Limpiar_SubBienes();
                if (Grid_Partes.SelectedIndex > (-1)) {
                    String Bien_ID = Grid_Partes.SelectedRow.Cells[1].Text.Trim();
                    Cls_Ope_Pat_Com_Bienes_Muebles_Negocio Bien = new Cls_Ope_Pat_Com_Bienes_Muebles_Negocio();
                    Bien.P_Bien_Mueble_ID = Bien_ID;
                    Bien = Bien.Consultar_Detalles_Bien_Mueble();
                    Txt_Numero_Inventario_Parte.Text = Bien.P_Numero_Inventario;
                    Txt_Nombre_Parte.Text = Bien.P_Nombre_Producto.ToString();
                    Cmb_Material_Parte.SelectedIndex = Cmb_Material_Parte.Items.IndexOf(Cmb_Material_Parte.Items.FindByValue(Bien.P_Material_ID));
                    Cmb_Color_Parte.SelectedIndex = Cmb_Color_Parte.Items.IndexOf(Cmb_Color_Parte.Items.FindByValue(Bien.P_Color_ID));
                    Txt_Costo_Parte.Text = Bien.P_Costo_Actual.ToString();
                    Txt_Fecha_Adquisicion_Parte.Text = String.Format("{0:dd 'de' MMMMMMMMMMMMMMM 'de' yyyy}", Convert.ToDateTime(Bien.P_Fecha_Adquisicion));
                    Cmb_Estatus_Parte.SelectedIndex = Cmb_Estatus_Parte.Items.IndexOf(Cmb_Estatus_Parte.Items.FindByValue(Bien.P_Estatus));
                    Cmb_Estado_Parte.SelectedIndex = Cmb_Estado_Parte.Items.IndexOf(Cmb_Estado_Parte.Items.FindByValue(Bien.P_Estado));
                    Txt_Comentarios_Parte.Text = Bien.P_Observaciones;
                    System.Threading.Thread.Sleep(1000);
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
                        //Hdf_Proveedor_ID.Value = Dt_Proveedor_Seleccionado.Rows[0][Cat_Com_Proveedores.Campo_Proveedor_ID].ToString().Trim();
                        Hdf_Proveedor_ID.Value = Proveedor_ID.Trim();

                        if (Dt_Proveedor_Seleccionado.Rows[0][Cat_Com_Proveedores.Campo_Nombre].ToString().Trim() != "")
                            Txt_Nombre_Proveedor.Text = Dt_Proveedor_Seleccionado.Rows[0][Cat_Com_Proveedores.Campo_Nombre].ToString().Trim();
                        else
                            Txt_Nombre_Proveedor.Text = "SIN NOMBRE";

                        //Upd_Panel.Update();
                        Mpe_Proveedores_Cabecera.Hide();
                    }
                    System.Threading.Thread.Sleep(500);
                    //Grid_Listado_Productos.SelectedIndex = (-1);
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
        protected void Grid_Listado_Proveedores_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                Grid_Listado_Proveedores.SelectedIndex = (-1);
                Llenar_Grid_Proveedores(e.NewPageIndex);
            }
            catch (Exception Ex)
            {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Resultados_Multiples_SelectedIndexChanged
        ///DESCRIPCIÓN: Maneja el evento de cambio de Seleccion del GridView de Bienes del
        ///             Modal de Resultados Multiples.
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 13/Septiembre/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************
        protected void Grid_Resultados_Multiples_SelectedIndexChanged(object sender, EventArgs e) {
            try{
                if (Grid_Resultados_Multiples.SelectedIndex > (-1)) {
                    Limpiar_Generales();
                    String Bien_Mueble_Seleccionado_ID = Grid_Resultados_Multiples.SelectedRow.Cells[1].Text.Trim();
                    Cls_Ope_Pat_Com_Bienes_Muebles_Negocio Bien_Mueble = new Cls_Ope_Pat_Com_Bienes_Muebles_Negocio();
                    Bien_Mueble.P_Bien_Mueble_ID = Bien_Mueble_Seleccionado_ID;
                    Bien_Mueble = Bien_Mueble.Consultar_Detalles_Bien_Mueble();
                    Mostrar_Detalles_Bien_Mueble(Bien_Mueble);
                    Grid_Listado_Bienes.SelectedIndex = -1;
                    Mpe_Multiples_Resultados.Hide();
                    System.Threading.Thread.Sleep(500);
                }
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }            
        }
        
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Resultados_Multiples_PageIndexChanging
        ///DESCRIPCIÓN: Maneja la paginación del GridView de Resultados Multiples.
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 13/Diciembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Resultados_Multiples_PageIndexChanging(object sender, GridViewPageEventArgs e) {
            try {
                Grid_Resultados_Multiples.PageIndex = e.NewPageIndex;
                Btn_Buscar_Click(Btn_Buscar, null);
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

    #endregion

    #region Eventos

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
            } catch (Exception Ex) {
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
        ///FECHA_CREO: 01/Diciembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Cmb_Busqueda_Resguardantes_Dependencias_SelectedIndexChanged(object sender, EventArgs e) { 
            try{
                if (Cmb_Busqueda_Resguardantes_Dependencias.SelectedIndex > 0) {
                    Cls_Ope_Pat_Com_Bienes_Muebles_Negocio Combo = new Cls_Ope_Pat_Com_Bienes_Muebles_Negocio();
                    Combo.P_Tipo_DataTable = "EMPLEADOS";
                    Combo.P_Dependencia_ID = Cmb_Busqueda_Resguardantes_Dependencias.SelectedItem.Value.Trim();
                    DataTable Tabla = Combo.Consultar_DataTable();
                    Llenar_Combo_Empleados_Busqueda(Tabla);
                } else {
                    DataTable Tabla = new DataTable();
                    Tabla.Columns.Add("EMPLEADO_ID", Type.GetType("System.String"));
                    Tabla.Columns.Add("NOMBRE", Type.GetType("System.String"));
                    Llenar_Combo_Empleados_Busqueda(Tabla);
                }
                MPE_Busqueda_Bien_Mueble.Show();
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }
    
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Modificar_Click
        ///DESCRIPCIÓN: Prepara y Actualiza un Bien Mueble con uno o mas resguardantes.
        ///PROPIEDADES:     
        ///CREO:                Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO:         02/Diciembre/2010 
        ///MODIFICO:           Salvador Hernández Ramírez
        ///FECHA_MODIFICO      24-Agosto-11
        ///CAUSA_MODIFICACIÓN  Se agregó la condición para que se actualice el resguardo o recibo
        ///*******************************************************************************
        protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e) {
            try {
                if (Btn_Modificar.AlternateText.Equals("Modificar")) {
                    if (Hdf_Bien_Mueble_ID.Value.Trim().Length > 0) {
                        if (!Cmb_Estatus.SelectedItem.Value.Equals("DEFINITIVA")) {
                            Configuracion_Formulario(false);              
                        } else {
                            Lbl_Ecabezado_Mensaje.Text = "";
                            Lbl_Mensaje_Error.Text = "El Estatus del Bien Mueble es \"BAJA DEFINITIVA\" y no puede ser actualizado el Bien";
                            Div_Contenedor_Msj_Error.Visible = true;
                        }
                    } else {
                        Lbl_Ecabezado_Mensaje.Text = "Es necesario.";
                        Lbl_Mensaje_Error.Text = "Seleccionar el Bien a Modificar";
                        Div_Contenedor_Msj_Error.Visible = true;                    
                    }
                } else { // Si se va a guardar
                    if (Validar_Componentes_Generales()) {
                        if (Validar_Baja_Bien()) { 
                            Cls_Ope_Pat_Com_Bienes_Muebles_Negocio Bien_Mueble = new Cls_Ope_Pat_Com_Bienes_Muebles_Negocio();
                            Bien_Mueble.P_Bien_Mueble_ID = Hdf_Bien_Mueble_ID.Value.Trim();
                            Bien_Mueble.P_Dependencia_ID = Cmb_Dependencias.SelectedItem.Value.Trim();
                           
                            if (Cmb_Marca.SelectedIndex!= 0)
                                Bien_Mueble.P_Marca_ID = Cmb_Marca.SelectedItem.Value.Trim();
                            else
                                Bien_Mueble.P_Marca_ID = null;

                            if (Cmb_Zonas.SelectedIndex != 0)
                                Bien_Mueble.P_Zona = Cmb_Zonas.SelectedItem.Value.Trim();
                            else
                                Bien_Mueble.P_Zona = null;

                            //Bien_Mueble.P_Modelo_ID = Cmb_Modelo.SelectedItem.Value.Trim();
                            Bien_Mueble.P_Clase_Activo_ID = Cmb_Clase_Activo.SelectedItem.Value.Trim();
                            Bien_Mueble.P_Clasificacion_ID = Cmb_Tipo_Activo.SelectedItem.Value.Trim();
                            Bien_Mueble.P_Modelo = Txt_Modelo.Text.Trim();
                            Bien_Mueble.P_Garantia = Txt_Garantia.Text.Trim();
                            Bien_Mueble.P_Material_ID = Cmb_Materiales.SelectedItem.Value.Trim();
                            Bien_Mueble.P_Color_ID = Cmb_Colores.SelectedItem.Value.Trim();
                            Bien_Mueble.P_Fecha_Adquisicion_ = Convert.ToDateTime(Txt_Fecha_Adquisicion.Text.Trim());
                            Bien_Mueble.P_Factura = Txt_Factura.Text.Trim();
                            Bien_Mueble.P_Numero_Serie = Txt_Numero_Serie.Text.Trim();
                            Bien_Mueble.P_Estatus = Cmb_Estatus.SelectedItem.Value.Trim();
                            if (!Bien_Mueble.P_Estatus.Equals("VIGENTE")) {
                                Bien_Mueble.P_Motivo_Baja = Txt_Motivo_Baja.Text;
                            }
                            Bien_Mueble.P_Procedencia = Cmb_Procedencia.SelectedItem.Value.Trim();
                            Bien_Mueble.P_Proveedor_ID = Hdf_Proveedor_ID.Value.Trim();
                            Bien_Mueble.P_Estado = Cmb_Estado.SelectedItem.Value.Trim();
                            Bien_Mueble.P_Observaciones = Txt_Observaciones.Text.Trim();
                            if (AFU_Archivo.HasFile) {
                                Bien_Mueble.P_Archivo = AFU_Archivo.FileName;
                            }
                            Bien_Mueble.P_Resguardantes = (DataTable)Session["Dt_Resguardantes"];
                            Bien_Mueble.P_Usuario_Nombre = Cls_Sessiones.Nombre_Empleado;
                            Bien_Mueble.P_Usuario_ID = Cls_Sessiones.Empleado_ID;

                            if (Session["OPERACION_BM"]!= null)
                                    Bien_Mueble.P_Operacion = Session["OPERACION_BM"].ToString().Trim();

                            Bien_Mueble.Modificar_Bien_Mueble();  // Se instancia el método que se utilizará para modificar el bien mueble
                            if (AFU_Archivo.HasFile) {
                                String Ruta = Server.MapPath("../../" + Ope_Pat_Archivos_Bienes.Campo_Ruta_Fisica_Archivos + "/BIENES_MUEBLES/" + Bien_Mueble.P_Bien_Mueble_ID);
                                if (!Directory.Exists(Ruta)) {
                                    Directory.CreateDirectory(Ruta);
                                }
                                String Archivo = Ruta + "/" + Bien_Mueble.P_Archivo;
                                AFU_Archivo.SaveAs(Archivo);
                            }
                            Configuracion_Formulario(true);
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Actualización de Bienes Muebles", "alert('Actualización de Bien Mueble Exitosa');", true);
                            Bien_Mueble = new Cls_Ope_Pat_Com_Bienes_Muebles_Negocio();
                            Bien_Mueble.P_Bien_Mueble_ID = Hdf_Bien_Mueble_ID.Value.Trim();
                            Bien_Mueble = Bien_Mueble.Consultar_Detalles_Bien_Mueble();
                            Mostrar_Detalles_Bien_Mueble(Bien_Mueble);
                        }
                    }
                }
            } catch (Exception Ex) {
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
        ///FECHA_CREO: 02/Diciembre/2010 
        ///MODIFICO:    
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Salir_Click(object sender, ImageClickEventArgs e) {
            if (Btn_Salir.AlternateText.Equals("Salir")) {
                Session["Dt_Resguardantes"] = null;
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
            } else {
                Configuracion_Formulario(true);
                Tab_Contenedor_Pestagnas.TabIndex = 0;
            }
        }
    
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Avanzada_Click
        ///DESCRIPCIÓN: Carga el Modal Popup de Busqueda Avanzada.
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 11/Diciembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Avanzada_Click(object sender, EventArgs e) {
            Div_Contenedor_Msj_Error.Visible = false;
            Lbl_Mensaje_Error.Text = "";
            Pnl_Busqueda_Bien_Mueble.Visible = true;
            MPE_Busqueda_Bien_Mueble.Show();
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Click
        ///DESCRIPCIÓN: Carga el Modal Popup de Busqueda Directa.
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 13/Diciembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e) {
            try {
                if (Txt_Busqueda.Text.Trim().Length > 0 || Txt_Busqueda_Anterior.Text.Trim().Length > 0) {
                    Limpiar_Generales();
                    Boolean Multiples_Resultados = false;
                    String No_Inventario_SIAS = Txt_Busqueda.Text.Trim();
                    String No_Inventario_Anterior = Txt_Busqueda_Anterior.Text.Trim();
                    if (No_Inventario_Anterior.Trim().Length > 0 || No_Inventario_SIAS.Trim().Length > 0) {
                        Cls_Ope_Pat_Com_Bienes_Muebles_Negocio Bien_Mueble_Negocio = new Cls_Ope_Pat_Com_Bienes_Muebles_Negocio();
                        Bien_Mueble_Negocio.P_Tipo_DataTable = "BIENES";
                        Bien_Mueble_Negocio.P_Numero_Inventario_Anterior = No_Inventario_Anterior.Trim();
                        Bien_Mueble_Negocio.P_Numero_Inventario = No_Inventario_SIAS.Trim();
                        DataTable Dt_Temporal = Bien_Mueble_Negocio.Consultar_DataTable();
                        if (Dt_Temporal.Rows.Count > 1) {
                            Multiples_Resultados = true;
                            Llenar_Grid_Resultados_Multiples(Dt_Temporal);
                            Mpe_Multiples_Resultados.Show();
                            return;
                        } else if (Dt_Temporal.Rows.Count == 1) {
                            No_Inventario_Anterior = Dt_Temporal.Rows[0]["NO_INVENTARIO_ANTERIOR"].ToString().Trim();
                        }
                    }
                    if (!Multiples_Resultados) {
                        Cls_Ope_Pat_Com_Bienes_Muebles_Negocio Bien_Mueble = new Cls_Ope_Pat_Com_Bienes_Muebles_Negocio();
                        Bien_Mueble.P_Numero_Inventario = No_Inventario_SIAS;
                        Bien_Mueble.P_Numero_Inventario_Anterior = No_Inventario_Anterior;
                        Bien_Mueble.P_Buscar_Numero_Inventario = true;
                        Bien_Mueble = Bien_Mueble.Consultar_Detalles_Bien_Mueble();

                        String Operacion = Bien_Mueble.P_Operacion;
                        Txt_Resguardo_Recibo.Text = Operacion;

                        if (Operacion != null)
                            Session["OPERACION_BM"] = Operacion;
                        else
                            Session["OPERACION_BM"] = null;

                        if (Bien_Mueble.P_Bien_Mueble_ID != null && Bien_Mueble.P_Bien_Mueble_ID.Trim().Length > 0) {
                            Mostrar_Detalles_Bien_Mueble(Bien_Mueble);
                        } else {
                            Lbl_Ecabezado_Mensaje.Text = HttpUtility.HtmlDecode("No se encontro un Bien Mueble con el Número de Inventario.");
                            Lbl_Mensaje_Error.Text = "";
                            Div_Contenedor_Msj_Error.Visible = true;
                        } 
                    } 
                } else {
                    Lbl_Ecabezado_Mensaje.Text = "Es necesario.";
                    Lbl_Mensaje_Error.Text = "Introducir el Número de Inventario a Buscar";
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
        ///NOMBRE DE LA FUNCIÓN: Btn_Ver_Archivo_Click
        ///DESCRIPCIÓN: Limpia los componentes del MPE de Cancelación de Vacuna
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 16/Febrero/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Ver_Archivo_Click(object sender, ImageClickEventArgs e) {
            try {
                ImageButton Boton = (ImageButton)sender;
                String Archivo_Bien_ID = Boton.CommandArgument;
                for (Int32 Contador = 0; Contador < Grid_Archivos.Rows.Count; Contador++) {
                    if (Grid_Archivos.Rows[Contador].Cells[0].Text.Trim().Equals(Archivo_Bien_ID)) {
                        String Archivo = "../../" + Ope_Pat_Archivos_Bienes.Campo_Ruta_Fisica_Archivos + "/BIENES_MUEBLES/" + Hdf_Bien_Mueble_ID.Value + "/" + Grid_Archivos.Rows[Contador].Cells[1].Text.Trim();
                        if (File.Exists(Server.MapPath(Archivo))) {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Window_Archivo_Archivos", "window.open('" + Archivo + "','Window_Archivo','left=0,top=0')", true);
                            break;
                        } else {
                            Lbl_Ecabezado_Mensaje.Text = "El Archivo no esta disponible o fue eliminado";
                            Lbl_Mensaje_Error.Text = "";
                            Div_Contenedor_Msj_Error.Visible = true;
                        }
                    }
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
                if (Btn_Modificar.AlternateText.Equals("Modificar")) {
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
                }
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = false;
            }
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
        protected void Cmb_Dependencias_SelectedIndexChanged(object sender, EventArgs e) {
            String Depencencia_Id = Cmb_Dependencias.SelectedItem.Value.Trim();
            Consultar_Empleados(Depencencia_Id);
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

        #region Resguardos

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Btn_Agregar_Resguardante_Click
            ///DESCRIPCIÓN: Agrega una nuevo Empleado Resguardante para este Bien Mueble.
            ///             (No aun en la Base de Datos)
            ///PROPIEDADES:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 02/Diciembre/2010 
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
                            Fila["BIEN_RESGUARDO_ID"] = 0;
                            Fila["EMPLEADO_ID"] = Dt_Empleado.Rows[0][Cat_Empleados.Campo_Empleado_ID].ToString().Trim();
                            Fila["NO_EMPLEADO"] = Dt_Empleado.Rows[0][Cat_Empleados.Campo_No_Empleado].ToString().Trim();
                            Fila["NOMBRE_EMPLEADO"] = HttpUtility.HtmlDecode(Cmb_Empleados.SelectedItem.Text);
                            Fila["COMENTARIOS"] = HttpUtility.HtmlDecode(Txt_Cometarios.Text.Trim());
                            Tabla.Rows.Add(Fila);
                        }
                        Llenar_Grid_Resguardantes(Grid_Resguardantes.PageIndex, Tabla);
                        Grid_Resguardantes.SelectedIndex = (-1);
                        Cmb_Empleados.SelectedIndex = 0;
                        Txt_Cometarios.Text = "";
                    } else {
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
            ///FECHA_CREO: 02/Diciembre/2010 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            protected void Btn_Quitar_Resguardante_Click(object sender, ImageClickEventArgs e){
                if (Grid_Resguardantes.Rows.Count > 0 && Grid_Resguardantes.SelectedIndex > (-1)) {
                    Int32 Registro = ((Grid_Resguardantes.PageIndex) * Grid_Resguardantes.PageSize) + (Grid_Resguardantes.SelectedIndex);
                    if (Session["Dt_Resguardantes"] != null) {
                        DataTable Tabla = (DataTable)Session["Dt_Resguardantes"];
                        Tabla.Rows.RemoveAt(Registro);
                        Session["Dt_Resguardantes"] = Tabla;
                        Grid_Resguardantes.SelectedIndex = (-1);
                        Llenar_Grid_Resguardantes(Grid_Resguardantes.PageIndex, Tabla);
                    }
                } else {
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
            protected void Btn_Limpiar_Filtros_Buscar_Datos_Click(object sender, ImageClickEventArgs e) {
                try {
                    Txt_Busqueda_Producto.Text = "";
                    Cmb_Busqueda_Dependencias.SelectedIndex = 0;
                    Txt_Busqueda_Modelo.Text = "";
                    Cmb_Busqueda_Marca.SelectedIndex = 0;
                    Cmb_Busqueda_Estatus.SelectedIndex = 0;
                    Txt_Busqueda_Numero_Serie.Text = "";
                    Txt_Busqueda_Factura.Text = "";
                    MPE_Busqueda_Bien_Mueble.Show();
                } catch (Exception Ex) {
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
            protected void Btn_Buscar_Datos_Click(object sender, ImageClickEventArgs e) {
                try {
                    Session["FILTRO_BUSQUEDA"] = "DATOS_GENERALES";
                    Llenar_Grid_Listado_Bienes(0);
                } catch (Exception Ex) {
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
            protected void Btn_Limpiar_Filtros_Buscar_Resguardante_Click(object sender, ImageClickEventArgs e) {
                try {
                    Txt_Busqueda_RFC_Resguardante.Text = "";
                    Cmb_Busqueda_Nombre_Resguardante.SelectedIndex = 0;
                    Cmb_Busqueda_Resguardantes_Dependencias.SelectedIndex = 0;
                    DataTable Tabla = new DataTable();
                    Tabla.Columns.Add("EMPLEADO_ID", Type.GetType("System.String"));
                    Tabla.Columns.Add("NOMBRE", Type.GetType("System.String"));
                    Llenar_Combo_Empleados_Busqueda(Tabla);
                    MPE_Busqueda_Bien_Mueble.Show();
                } catch (Exception Ex) {
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
            protected void Btn_Buscar_Resguardante_Click(object sender, ImageClickEventArgs e) {
                try {
                    Session["FILTRO_BUSQUEDA"] = "RESGUARDANTES";
                    Llenar_Grid_Listado_Bienes(0);
                }catch (Exception Ex) {
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

    #endregion

}