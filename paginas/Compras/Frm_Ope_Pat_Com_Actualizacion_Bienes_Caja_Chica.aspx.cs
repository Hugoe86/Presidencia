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
using Presidencia.Control_Patrimonial_Operacion_Bienes_Caja_Chica.Negocio;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using System.IO;
using System.Collections.Generic;

public partial class paginas_Compras_Frm_Ope_Pat_Com_Actualizacion_Bienes_Caja_Chica : System.Web.UI.Page
{

    #region Page_Load

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Page_Load
        ///DESCRIPCIÓN: Metodo que se carga cada que ocurre un PostBack de la Página
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 25/Enero/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************  
        protected void Page_Load(object sender, EventArgs e) {
            Div_Contenedor_Msj_Error.Visible = false;
            if (!IsPostBack) {
                Llenar_Combos_Dependencias();
                Llenar_Combos_Materiales();
                Llenar_Combo_Colores();
                Llenar_Combo_Marcas();
                Llenar_Combo_Modelos();
                Grid_Listado_Bienes.Columns[1].Visible = false;
                Configuracion_Formulario(true);
            }
        }

    #endregion

    #region Metodos

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Llenar_Combos_Dependencias
        ///DESCRIPCIÓN: Se llenan los Combos Generales Independientes.
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 24/Enero/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Llenar_Combos_Dependencias() {
            try {
                Cls_Ope_Pat_Com_Bienes_Caja_Chica_Negocio Combos = new Cls_Ope_Pat_Com_Bienes_Caja_Chica_Negocio();

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
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Llenar_Combos_Materiales
        ///DESCRIPCIÓN: Se llenan los Combos Materiales.
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 11/Febrero/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Llenar_Combos_Materiales() {
            try {
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
                Materiales.Rows.RemoveAt(0);
                Fila_Material["MATERIAL_ID"] = "TODOS";
                Fila_Material["DESCRIPCION"] = HttpUtility.HtmlDecode("&lt;TODOS&gt;");
                Materiales.Rows.InsertAt(Fila_Material, 0);
                Cmb_Busqueda_Material.DataSource = Materiales;
                Cmb_Busqueda_Material.DataValueField = "MATERIAL_ID";
                Cmb_Busqueda_Material.DataTextField = "DESCRIPCION";
                Cmb_Busqueda_Material.DataBind();
            } catch (Exception Ex) {
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
                Colores.Rows.RemoveAt(0);
                Fila_Color["COLOR_ID"] = "TODOS";
                Fila_Color["DESCRIPCION"] = HttpUtility.HtmlDecode("&lt;TODOS&gt;");
                Colores.Rows.InsertAt(Fila_Color, 0);
                Cmb_Busqueda_Color.DataSource = Colores;
                Cmb_Busqueda_Color.DataValueField = "COLOR_ID";
                Cmb_Busqueda_Color.DataTextField = "DESCRIPCION";
                Cmb_Busqueda_Color.DataBind();
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
                Marcas.Rows.RemoveAt(0);
                Fila_Marca["MARCA_ID"] = "TODOS";
                Fila_Marca["NOMBRE"] = HttpUtility.HtmlDecode("&lt;TODOS&gt;");
                Marcas.Rows.InsertAt(Fila_Marca, 0);
                Cmb_Busqueda_Marca.DataSource = Marcas;
                Cmb_Busqueda_Marca.DataValueField = "MARCA_ID";
                Cmb_Busqueda_Marca.DataTextField = "NOMBRE";
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
                Modelos.Rows.RemoveAt(0);
                Fila_Modelo["MODELO_ID"] = "TODOS";
                Fila_Modelo["NOMBRE"] = HttpUtility.HtmlDecode("&lt;TODOS&gt;");
                Modelos.Rows.InsertAt(Fila_Modelo, 0);
                Cmb_Busqueda_Modelo.DataSource = Modelos;
                Cmb_Busqueda_Modelo.DataValueField = "MODELO_ID";
                Cmb_Busqueda_Modelo.DataTextField = "NOMBRE";
                Cmb_Busqueda_Modelo.DataBind();
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
        ///FECHA_CREO: 25/Enero/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Llenar_Combo_Empleados() {
            try {
                DataTable Tabla = new DataTable();
                if (Hdf_Bien_ID.Value.Trim().Length > 0) {
                    Cls_Ope_Pat_Com_Bienes_Caja_Chica_Negocio Empleados = new Cls_Ope_Pat_Com_Bienes_Caja_Chica_Negocio();
                    Empleados.P_Tipo_DataTable = "EMPLEADOS_BIEN";
                    Empleados.P_Bien_ID = Hdf_Bien_ID.Value.Trim();
                    Tabla = Empleados.Consultar_DataTable();            
                }else{
                    Tabla.Columns.Add("EMPLEADO_ID", Type.GetType("System.String"));
                    Tabla.Columns.Add("NOMBRE", Type.GetType("System.String"));             
                }
                DataRow Fila_Empleado = Tabla.NewRow();
                Fila_Empleado["EMPLEADO_ID"] = "SELECCIONE";
                Fila_Empleado["NOMBRE"] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
                Tabla.Rows.InsertAt(Fila_Empleado, 0);
                Cmb_Empleados.DataSource = Tabla;
                Cmb_Empleados.DataValueField = "EMPLEADO_ID";
                Cmb_Empleados.DataTextField = "NOMBRE";
                Cmb_Empleados.DataBind();          
            } catch (Exception Ex) {
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
        ///FECHA_CREO: 25/Enero/2011
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
        ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Listado_Bienes
        ///DESCRIPCIÓN: Se llenan el Grid de Bienes del Modal de Busqueda dependiendo de 
        ///             los filtros pasados.
        ///PROPIEDADES:     
        ///             1. Pagina.  Pagina en donde aparecerá el Grid.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO:25/Enero/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Llenar_Grid_Listado_Bienes(Int32 Pagina) {
            try{
                Grid_Listado_Bienes.Columns[1].Visible = true;
                Cls_Ope_Pat_Com_Bienes_Caja_Chica_Negocio Bienes = new Cls_Ope_Pat_Com_Bienes_Caja_Chica_Negocio();
                Bienes.P_Tipo_DataTable = "BIENES";
                if (Session["FILTRO_BUSQUEDA"] != null) {
                    Bienes.P_Tipo_Filtro_Busqueda = Session["FILTRO_BUSQUEDA"].ToString();
                    if (Session["FILTRO_BUSQUEDA"].ToString().Trim().Equals("DATOS_GENERALES")) {
                        Bienes.P_Nombre = Txt_Busqueda_Nombre.Text.Trim();
                        if (Cmb_Busqueda_Material.SelectedIndex > 0) {
                            Bienes.P_Material_ID = Cmb_Busqueda_Material.SelectedItem.Value.Trim();
                        }
                        if (Cmb_Busqueda_Color.SelectedIndex > 0) {
                            Bienes.P_Color_ID = Cmb_Busqueda_Color.SelectedItem.Value.Trim();
                        }
                        if (Cmb_Busqueda_Marca.SelectedIndex > 0) {
                            Bienes.P_Marca_ID = Cmb_Busqueda_Marca.SelectedItem.Value.Trim();
                        }
                        if (Cmb_Busqueda_Modelo.SelectedIndex > 0)
                        {
                            Bienes.P_Modelo_ID = Cmb_Busqueda_Modelo.SelectedItem.Value.Trim();
                        }
                        if (Cmb_Busqueda_Estatus.SelectedIndex > 0) {
                            Bienes.P_Estatus = Cmb_Busqueda_Estatus.SelectedItem.Value.Trim();
                        }
                        if (Cmb_Busqueda_Dependencias.SelectedIndex > 0) {
                            Bienes.P_Dependencia_ID = Cmb_Busqueda_Dependencias.SelectedItem.Value.Trim();
                        }
                        if (Txt_Busqueda_Fecha_Aquisicion.Text.Trim().Length > 0) {
                            Bienes.P_Fecha_Adquisicion = Convert.ToDateTime(Txt_Busqueda_Fecha_Aquisicion.Text.Trim());
                            Bienes.P_Buscar_Fecha_Adquisicion = true;
                        }
                    } else if (Session["FILTRO_BUSQUEDA"].ToString().Trim().Equals("RESGUARDANTES")) {
                        Bienes.P_RFC_Resguardante = Txt_Busqueda_RFC_Resguardante.Text.Trim();
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
                MPE_Busqueda_Bien_Caja_Chica.Show();
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }
        
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Limpiar_Generales
        ///DESCRIPCIÓN: Se Limpian los campos Generales de los Bienes.
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO:  25/Enero/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Limpiar_Generales() {
            try {
                Hdf_Bien_ID.Value = "";
                Txt_Bien_ID.Text = "";
                Txt_Nombre.Text = "";
                Txt_Dependencia.Text = "";
                Txt_Numero_Inventario.Text = "";
                Txt_Cantidad.Text = "";
                Cmb_Material.SelectedIndex = 0;
                Cmb_Color.SelectedIndex = 0;
                Cmb_Marca.SelectedIndex = 0;
                Cmb_Modelo.SelectedIndex = 0;
                Txt_Costo.Text = "";
                Txt_Fecha_Adquisicion.Text = "";
                Cmb_Estatus.SelectedIndex = 0;
                Cmb_Estado.SelectedIndex = 0;
                Txt_Motivo_Baja.Text = "";
                Txt_Comentarios_Generales.Text = "";
                Grid_Resguardantes.DataSource = new DataTable();
                Grid_Resguardantes.DataBind();
                Grid_Historial_Resguardantes.DataSource = new DataTable();
                Grid_Historial_Resguardantes.DataBind();
                Limpiar_Resguardantes();
                Limpiar_Historial_Resguardantes();
                Llenar_Combo_Empleados();
                Remover_Sesiones_Control_AsyncFileUpload(AFU_Archivo.ClientID);
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Limpiar_Resguardantes
        ///DESCRIPCIÓN: Se Limpian los campos de Resguardantes de los Bienes.
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO:  25/Enero/2011
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
        ///             Bienes.
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO:  25/Enero/2011
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
        ///NOMBRE DE LA FUNCIÓN: Buscar_Clave_DataTable
        ///DESCRIPCIÓN: Busca una Clave en un DataTable, si la encuentra Retorna 'true'
        ///             en caso contrario 'false'.
        ///PROPIEDADES:  
        ///             1.  Clave.  Clave que se buscara en el DataTable
        ///             2.  Tabla.  Datatable donde se va a buscar la clave.
        ///             3.  Columna.Columna del DataTable donde se va a buscar la clave.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 25/Enero/2011
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
        ///FECHA_CREO: 25/Enero/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Llenar_Grid_Resguardantes(Int32 Pagina, DataTable Tabla) {
            Grid_Resguardantes.Columns[1].Visible = true;
            Grid_Resguardantes.DataSource = Tabla;
            Grid_Resguardantes.PageIndex = Pagina;
            Grid_Resguardantes.DataBind();
            Grid_Resguardantes.Columns[1].Visible = false;
            Session["Dt_Resguardantes"] = Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Historial_Resguardos
        ///DESCRIPCIÓN: Llena la tabla de Historial de Resguardantes
        ///PROPIEDADES:     
        ///             1.  Pagina. Pagina en la cual se mostrará el Grid_VIew
        ///             2.  Tabla.  Tabla que se va a cargar en el Grid.    
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 25/Enero/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Llenar_Grid_Historial_Resguardos(Int32 Pagina, DataTable Tabla) {
            Grid_Historial_Resguardantes.Columns[1].Visible = true;
            Grid_Historial_Resguardantes.DataSource = Tabla;
            Grid_Historial_Resguardantes.PageIndex = Pagina;
            Grid_Historial_Resguardantes.DataBind();
            Grid_Historial_Resguardantes.Columns[1].Visible = false;
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
        ///NOMBRE DE LA FUNCIÓN: Configuracion_Formulario
        ///DESCRIPCIÓN: Carga una configuracion de los controles del Formulario
        ///PROPIEDADES:     
        ///             1. Estatus. Estatus en el que se cargara la configuración de los 
        ///                         controles.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 25/Enero/2011
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
            } else {
                Btn_Modificar.AlternateText = "Actualizar";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                Btn_Salir.AlternateText = "Cancelar";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
            }
            Txt_Nombre.Enabled = !Estatus;
            Txt_Cantidad.Enabled = !Estatus;
            Cmb_Material.Enabled = !Estatus;
            Cmb_Color.Enabled = !Estatus;
            Cmb_Marca.Enabled = !Estatus;
            Cmb_Modelo.Enabled = !Estatus;
            Cmb_Estatus.Enabled = !Estatus;
            Cmb_Estado.Enabled = !Estatus;
            Txt_Motivo_Baja.Enabled = !Estatus;
            Txt_Comentarios_Generales.Enabled = !Estatus;
            Cmb_Empleados.Enabled = !Estatus;
            Txt_Cometarios.Enabled = !Estatus;
            Btn_Agregar_Resguardante.Visible = !Estatus;
            Btn_Quitar_Resguardante.Visible = !Estatus;
            Grid_Resguardantes.Enabled = !Estatus;
            Div_Busqueda.Visible = Estatus;
            AFU_Archivo.Enabled = !Estatus;


            Configuracion_Acceso("Frm_Ope_Pat_Com_Actualizacion_Bienes_Caja_Chica.aspx");
            Configuracion_Acceso_LinkButton("Frm_Ope_Pat_Com_Actualizacion_Bienes_Caja_Chica.aspx");
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Mostrar_Detalles_Bien
        ///DESCRIPCIÓN: Carga una configuracion de los controles del Formulario
        ///PROPIEDADES:     
        ///             1. Bien. Contiene los parametros que se desean mostrar.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 25/Enero/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Mostrar_Detalles_Bien(Cls_Ope_Pat_Com_Bienes_Caja_Chica_Negocio Bien){
            try {
                Hdf_Bien_ID.Value = Bien.P_Bien_ID;
                Txt_Bien_ID.Text = Bien.P_Bien_ID;
                Txt_Nombre.Text = Bien.P_Nombre;
                Txt_Dependencia.Text = Bien.P_Dependencia_ID;
                Txt_Numero_Inventario.Text = Bien.P_Numero_Inventario;
                Txt_Cantidad.Text = Bien.P_Cantidad.ToString();
                Cmb_Material.SelectedIndex = Cmb_Material.Items.IndexOf(Cmb_Material.Items.FindByValue(Bien.P_Material_ID));
                Cmb_Color.SelectedIndex = Cmb_Color.Items.IndexOf(Cmb_Color.Items.FindByValue(Bien.P_Color_ID));
                Cmb_Marca.SelectedIndex = Cmb_Marca.Items.IndexOf(Cmb_Marca.Items.FindByValue(Bien.P_Marca_ID));
                Cmb_Modelo.SelectedIndex = Cmb_Modelo.Items.IndexOf(Cmb_Modelo.Items.FindByValue(Bien.P_Modelo_ID));
                Txt_Costo.Text = "$ " + Bien.P_Costo.ToString("#,###,###.00");
                Txt_Fecha_Adquisicion.Text = String.Format("{0:dd/MMM/yyyy}", Bien.P_Fecha_Adquisicion);
                Cmb_Estatus.SelectedIndex = Cmb_Estatus.Items.IndexOf(Cmb_Estatus.Items.FindByValue(Bien.P_Estatus));
                Txt_Motivo_Baja.Text = Bien.P_Motivo_Baja;
                Cmb_Estado.SelectedIndex = Cmb_Estado.Items.IndexOf(Cmb_Estado.Items.FindByValue(Bien.P_Estado));
                Txt_Comentarios_Generales.Text = Bien.P_Comentarios;
                Llenar_Grid_Resguardantes(0, Bien.P_Resguardantes);
                Llenar_Combo_Empleados();
                Llenar_Grid_Historial_Resguardos(0, Bien.P_Historial_Resguardos);
                Llenar_Grid_Historial_Archivos(0, Bien.P_Dt_Historial_Archivos);
                Tab_Contenedor_Pestagnas.ActiveTabIndex = 0;
                System.Threading.Thread.Sleep(1000);
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
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

        #region Validaciones

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Validar_Componentes_Generales
            ///DESCRIPCIÓN: Hace una validacion de que haya datos en los componentes antes de hacer
            ///             una operación de Actualizacion.
            ///PROPIEDADES:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 25/Enero/2011
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            private bool Validar_Componentes_Generales() {
                Lbl_Ecabezado_Mensaje.Text = "Es necesario.";
                String Mensaje_Error = "";
                Boolean Validacion = true;
                if (Txt_Nombre.Text.Trim().Length == 0) {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                    Mensaje_Error = Mensaje_Error + "+ Introducir el Nombre del Bien.";
                    Validacion = false;
                }
                if (Txt_Cantidad.Text.Trim().Length == 0) {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                    Mensaje_Error = Mensaje_Error + "+ Introducir la Cantidad.";
                    Validacion = false;
                } else {
                    if (Convert.ToInt32(Txt_Cantidad.Text.Trim()) == 0) {
                        if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                        Mensaje_Error = Mensaje_Error + "+ La Cantidad debe ser de minimo 1.";
                        Validacion = false;
                    }
                }
                if (Cmb_Material.SelectedIndex == 0) {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                    Mensaje_Error = Mensaje_Error + "+ Debe seleccionar una opción del Combo de Material.";
                    Validacion = false;
                }
                if (Cmb_Color.SelectedIndex == 0) {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                    Mensaje_Error = Mensaje_Error + "+ Debe seleccionar una opción del Combo de Color.";
                    Validacion = false;
                }
                if (Cmb_Marca.SelectedIndex == 0) {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                    Mensaje_Error = Mensaje_Error + "+ Debe seleccionar una opción del Combo de Marca.";
                    Validacion = false;
                }
                if (Cmb_Modelo.SelectedIndex == 0) {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                    Mensaje_Error = Mensaje_Error + "+ Debe seleccionar una opción del Combo de Modelo.";
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
                if (Txt_Comentarios_Generales.Text.Trim().Length > 0 && Txt_Comentarios_Generales.Text.Trim().Length > 500) {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                    Mensaje_Error = Mensaje_Error + "+ Verificar la longitud de las comentarios (Se pasa por " + (Txt_Comentarios_Generales.Text.Trim().Length - 500).ToString() + ").";
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
            ///FECHA_CREO: 25/Enero/2011
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

        #endregion
    
    #endregion

    #region Grid
           
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Listado_Bienes_PageIndexChanging
        ///DESCRIPCIÓN: Maneja la paginación del GridView de Bienes del Modal de Busqueda
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 25/Enero/2011
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
        ///FECHA_CREO: 25/Enero/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Listado_Bienes_SelectedIndexChanged(object sender, EventArgs e) {
            try{
                if (Grid_Listado_Bienes.SelectedIndex > (-1)) {
                    Limpiar_Generales();
                    String Bien_Seleccionado_ID = Grid_Listado_Bienes.SelectedRow.Cells[1].Text.Trim();
                    Cls_Ope_Pat_Com_Bienes_Caja_Chica_Negocio Bien = new Cls_Ope_Pat_Com_Bienes_Caja_Chica_Negocio();
                    Bien.P_Bien_ID = Bien_Seleccionado_ID;
                    Bien = Bien.Consultar_Datos_Bien_Caja_Chica();
                    Mostrar_Detalles_Bien(Bien);
                    Grid_Listado_Bienes.SelectedIndex = -1;
                    MPE_Busqueda_Bien_Caja_Chica.Hide();
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
        ///FECHA_CREO: 25/Enero/2011 
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
        ///FECHA_CREO: 25/Enero/2011 
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
                        Txt_Historial_Empleado_Resguardo.Text = Tabla.Rows[Registro][2].ToString().Trim();
                        Txt_Historial_Comentarios_Resguardo.Text = Tabla.Rows[Registro][3].ToString().Trim();
                        Txt_Historial_Fecha_Inicial_Resguardo.Text = String.Format("{0:dd 'de' MMMMMMMMMMMM 'de' yyyy}", Tabla.Rows[Registro][4]);
                        Txt_Historial_Fecha_Final_Resguardo.Text = String.Format("{0:dd 'de' MMMMMMMMMMMM 'de' yyyy}", Tabla.Rows[Registro][5]);
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
        ///FECHA_CREO: 25/Enero/2011 
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
        ///NOMBRE DE LA FUNCIÓN: Grid_Archivos_PageIndexChanging
        ///DESCRIPCIÓN: Maneja la paginación del GridView de Historial de Archivos
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 16/Febrero/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Archivos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                if (Session["Dt_Historial_Archivos"] != null)
                {
                    Grid_Archivos.SelectedIndex = (-1);
                    Llenar_Grid_Historial_Archivos(e.NewPageIndex, (DataTable)Session["Dt_Historial_Archivos"]);
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
        ///NOMBRE DE LA FUNCIÓN: Grid_Archivos_RowDataBound
        ///DESCRIPCIÓN: Maneja el evento de RowDataBound del Grid de Archivos
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 16/Febrero/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Archivos_RowDataBound(object sender, GridViewRowEventArgs e)
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    ImageButton Boton = (ImageButton)e.Row.FindControl("Btn_Ver_Archivo");
                    Boton.CommandArgument = e.Row.Cells[0].Text.Trim();
                }
            }

    #endregion

    #region Eventos

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Cmb_Busqueda_Resguardantes_Dependencias_SelectedIndexChanged
        ///DESCRIPCIÓN: Maneja el evento de cambio de Selección del Combo de Dependencias
        ///             del Modal de Busqueda (Parte de Resguardantes).
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 25/Enero/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Cmb_Busqueda_Resguardantes_Dependencias_SelectedIndexChanged(object sender, EventArgs e) { 
            try{
                if (Cmb_Busqueda_Resguardantes_Dependencias.SelectedIndex > 0) {
                    Cls_Ope_Pat_Com_Bienes_Caja_Chica_Negocio Combo = new Cls_Ope_Pat_Com_Bienes_Caja_Chica_Negocio();
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
                MPE_Busqueda_Bien_Caja_Chica.Show();
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }
    
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Modificar_Click
        ///DESCRIPCIÓN: Prepara y Actualiza un Bien con uno o mas resguardantes.
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 25/Enero/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e) {
            try {
                if (Btn_Modificar.AlternateText.Equals("Modificar")) {
                    if (Hdf_Bien_ID.Value.Trim().Length > 0) {
                        if (!Cmb_Estatus.SelectedItem.Value.Equals("DEFINITIVA")) {
                            Configuracion_Formulario(false);              
                        } else {
                            Lbl_Ecabezado_Mensaje.Text = "";
                            Lbl_Mensaje_Error.Text = "El Estatus del Bien es \"BAJA DEFINITIVA\" y no puede ser actualizado el Bien";
                            Div_Contenedor_Msj_Error.Visible = true;
                        }
                    } else {
                        Lbl_Ecabezado_Mensaje.Text = "Es necesario.";
                        Lbl_Mensaje_Error.Text = "Seleccionar el Bien a Modificar";
                        Div_Contenedor_Msj_Error.Visible = true;                    
                    }
                } else {
                    if (Validar_Componentes_Generales()) {
                        Cls_Ope_Pat_Com_Bienes_Caja_Chica_Negocio Bien = new Cls_Ope_Pat_Com_Bienes_Caja_Chica_Negocio();
                        Bien.P_Bien_ID = Hdf_Bien_ID.Value.Trim();
                        Bien.P_Nombre = Txt_Nombre.Text.Trim();
                        Bien.P_Cantidad = Convert.ToInt32(Txt_Cantidad.Text.Trim());
                        Bien.P_Material_ID = Cmb_Material.SelectedItem.Value.Trim();
                        Bien.P_Color_ID = Cmb_Color.SelectedItem.Value.Trim();
                        Bien.P_Marca_ID = Cmb_Marca.SelectedItem.Value.Trim();
                        Bien.P_Modelo_ID = Cmb_Modelo.SelectedItem.Value.Trim();
                        Bien.P_Estatus = Cmb_Estatus.SelectedItem.Value;
                        Bien.P_Estado = Cmb_Estado.SelectedItem.Value;
                        Bien.P_Motivo_Baja = Txt_Motivo_Baja.Text.Trim();
                        Bien.P_Comentarios = Txt_Comentarios_Generales.Text.Trim();
                        if (AFU_Archivo.HasFile) {
                            Bien.P_Archivo = AFU_Archivo.FileName;
                        }
                        Bien.P_Resguardantes = (DataTable)Session["Dt_Resguardantes"];
                        Bien.P_Usuario_Nombre = Cls_Sessiones.Nombre_Empleado;
                        Bien.P_Usuario_ID = Cls_Sessiones.Empleado_ID;
                        Bien.Modificar_Bien_Caja_Chica();
                        if (AFU_Archivo.HasFile) {
                            String Ruta = Server.MapPath("../../" + Ope_Pat_Archivos_Bienes.Campo_Ruta_Fisica_Archivos + "/CAJA_CHICA/" + Bien.P_Bien_ID);
                            if (!Directory.Exists(Ruta)) {
                                Directory.CreateDirectory(Ruta);
                            }
                            String Archivo = Ruta + "/" + Bien.P_Archivo;
                            AFU_Archivo.SaveAs(Archivo);
                        }
                        Configuracion_Formulario(true);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Actualización de Bienes", "alert('Actualización de Bien Exitosa');", true);
                        Bien = new Cls_Ope_Pat_Com_Bienes_Caja_Chica_Negocio();
                        Bien.P_Bien_ID = Hdf_Bien_ID.Value.Trim();
                        Bien = Bien.Consultar_Datos_Bien_Caja_Chica();
                        Mostrar_Detalles_Bien(Bien);
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
        ///FECHA_CREO: 25/Enero/2011 
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
                Cls_Ope_Pat_Com_Bienes_Caja_Chica_Negocio Bien = new Cls_Ope_Pat_Com_Bienes_Caja_Chica_Negocio();
                Bien.P_Bien_ID = Hdf_Bien_ID.Value.Trim();
                Bien = Bien.Consultar_Datos_Bien_Caja_Chica();
                Mostrar_Detalles_Bien(Bien);
            }
        }
    
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Avanzada_Click
        ///DESCRIPCIÓN: Carga el Modal Popup de Busqueda Avanzada.
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO:  25/Enero/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Avanzada_Click(object sender, EventArgs e) {
            Div_Contenedor_Msj_Error.Visible = false;
            Lbl_Mensaje_Error.Text = "";
            Pnl_Busqueda_Bien_Caja_Chica.Visible = true;
            MPE_Busqueda_Bien_Caja_Chica.Show();
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Click
        ///DESCRIPCIÓN: Carga el Modal Popup de Busqueda Directa.
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO:  25/Enero/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e) {
            try {
                if (Txt_Busqueda.Text.Trim().Length > 0) {
                    Limpiar_Generales();
                    String Clave_Inventario = Txt_Busqueda.Text.Trim();
                    Cls_Ope_Pat_Com_Bienes_Caja_Chica_Negocio Bien = new Cls_Ope_Pat_Com_Bienes_Caja_Chica_Negocio();
                    Bien.P_Numero_Inventario = Clave_Inventario;
                    Bien.P_Buscar_Numero_Inventario = true;
                    Bien = Bien.Consultar_Datos_Bien_Caja_Chica();
                    if (Bien.P_Bien_ID != null && Bien.P_Bien_ID.Trim().Length > 0) {
                        Mostrar_Detalles_Bien(Bien);
                    } else {
                        Lbl_Ecabezado_Mensaje.Text = HttpUtility.HtmlDecode("No se encontro un Bien con el Número de Inventario '" + Txt_Busqueda.Text.Trim() + "'.");
                        Lbl_Mensaje_Error.Text = "";
                        Div_Contenedor_Msj_Error.Visible = true;
                    }
                } else {
                    Lbl_Ecabezado_Mensaje.Text = "Es necesario.";
                    Lbl_Mensaje_Error.Text = "Introducir el Número de Inventario a Buscar";
                    Div_Contenedor_Msj_Error.Visible = true;                    
                }
            } catch (Exception Ex) {
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
        protected void Btn_Ver_Archivo_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton Boton = (ImageButton)sender;
                String Archivo_Bien_ID = Boton.CommandArgument;
                for (Int32 Contador = 0; Contador < Grid_Archivos.Rows.Count; Contador++)
                {
                    if (Grid_Archivos.Rows[Contador].Cells[0].Text.Trim().Equals(Archivo_Bien_ID))
                    {
                        String Archivo = Server.MapPath("../../" + Ope_Pat_Archivos_Bienes.Campo_Ruta_Fisica_Archivos + "/CAJA_CHICA/" + Hdf_Bien_ID.Value + "/" + Grid_Archivos.Rows[Contador].Cells[1].Text.Trim());
                        if (File.Exists(Archivo))
                        {
                            //ScriptManager.RegisterStartupScript(this, this.GetType(), "Window_Archivo", "window.open('" + Archivo + "','Window_Archivo','left=0,top=0')", true);
                            System.Diagnostics.Process proceso = new System.Diagnostics.Process();
                            proceso.StartInfo.FileName = Archivo;
                            proceso.Start();
                            proceso.Close();
                            break;
                        }
                        else
                        {
                            Lbl_Ecabezado_Mensaje.Text = "El Archivo no esta disponible o fue eliminado";
                            Lbl_Mensaje_Error.Text = "";
                            Div_Contenedor_Msj_Error.Visible = true;
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
                            Tabla.Columns.Add("NOMBRE_EMPLEADO", Type.GetType("System.String"));
                            Tabla.Columns.Add("COMENTARIOS", Type.GetType("System.String"));
                        } else {
                            Tabla = (DataTable)Session["Dt_Resguardantes"];
                        }
                    }
                    if (!Buscar_Clave_DataTable(Cmb_Empleados.SelectedItem.Value, Tabla, 1)) {
                        DataRow Fila = Tabla.NewRow();
                        Fila["BIEN_RESGUARDO_ID"] = 0;
                        Fila["EMPLEADO_ID"] = HttpUtility.HtmlDecode(Cmb_Empleados.SelectedItem.Value);
                        Fila["NOMBRE_EMPLEADO"] = HttpUtility.HtmlDecode(Cmb_Empleados.SelectedItem.Text);
                        Fila["COMENTARIOS"] = HttpUtility.HtmlDecode(Txt_Cometarios.Text.Trim());
                        Tabla.Rows.Add(Fila);
                        Grid_Resguardantes.DataSource = Tabla;
                        Session["Dt_Resguardantes"] = Tabla;
                        Grid_Resguardantes.DataBind();
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
            ///FECHA_CREO:  25/Enero/2011 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************    
            protected void Btn_Limpiar_Filtros_Buscar_Datos_Click(object sender, ImageClickEventArgs e) {
                try {
                    Txt_Busqueda_Nombre.Text = "";
                    Cmb_Busqueda_Material.SelectedIndex = 0;
                    Cmb_Busqueda_Color.SelectedIndex = 0;
                    Cmb_Busqueda_Marca.SelectedIndex = 0;
                    Cmb_Busqueda_Modelo.SelectedIndex = 0;
                    Cmb_Busqueda_Estatus.SelectedIndex = 0;
                    Txt_Busqueda_Fecha_Aquisicion.Text = "";
                    MPE_Busqueda_Bien_Caja_Chica.Show();
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
            ///FECHA_CREO: 25/Enero/2011 
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
            ///FECHA_CREO:  25/Enero/2011 
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
                    MPE_Busqueda_Bien_Caja_Chica.Show();
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
            ///FECHA_CREO:  25/Enero/2011 
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
                    Botones.Add(Btn_Modificar);
                    Botones.Add(Btn_Buscar);

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
            protected void Configuracion_Acceso_LinkButton(String URL_Pagina)
            {
                List<LinkButton> Botones = new List<LinkButton>();//Variable que almacenara una lista de los botones de la página.
                DataRow[] Dr_Menus = null;//Variable que guardara los menus consultados.

                try
                {
                    //Agregamos los botones a la lista de botones de la página.
                    Botones.Add(Btn_Busqueda_Avanzada);

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
                                    Cls_Util.Configuracion_Acceso_Sistema_SIAS(Botones, Dr_Menus[0]);//Habilitamos la configuracón de los botones.
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